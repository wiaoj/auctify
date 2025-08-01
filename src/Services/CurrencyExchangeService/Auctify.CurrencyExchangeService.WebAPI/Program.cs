using System.Net;
using Auctify.CurrencyExchangeService.WebAPI.BackgroundServices;
using Auctify.CurrencyExchangeService.WebAPI.Constants;
using Auctify.CurrencyExchangeService.WebAPI.Jobs;
using Auctify.CurrencyExchangeService.WebAPI.Services;
using Auctify.CurrencyExchangeService.WebAPI.Services.Abstractions;
using Auctify.Libraries.Domain.ValueObjects;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? redisConnectionString = builder.Configuration.GetConnectionString("Redis");
string? redisPassword = builder.Configuration["Redis:Password"];

if(string.IsNullOrWhiteSpace(redisConnectionString) || string.IsNullOrWhiteSpace(redisPassword))
    throw new InvalidOperationException("Redis connection string is not configured.");

ConfigurationOptions options = new() {
    EndPoints = { { redisConnectionString } },
    Password = redisPassword,
    AbortOnConnectFail = false
};

ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(options);

builder.Services.AddSingleton<IConnectionMultiplexer>(muxer);

builder.Services.AddHangfire(config =>
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseRedisStorage(muxer));

builder.Services.AddHangfireServer(options => {
    options.WorkerCount = 2;
});


builder.Services.AddHttpClient<IBaseRateProvider, TcmbRateProvider>(client => {
    client.BaseAddress = new Uri(TcmbConstants.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/xml");
})
  .SetHandlerLifetime(TimeSpan.FromMinutes(1))
  .AddPolicyHandler(GetRetryPolicy());

builder.Services.TryAddSingleton<TimeProvider>(TimeProvider.System);
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICrossRateCalculator, CrossRateCalculator>();
builder.Services.AddScoped<IExchangeRateRepository, RedisExchangeRateRepository>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

builder.Services.AddHostedService<InitialRateFetcherService>();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");

app.MapGet("/", async (IExchangeRateService rateService) => {
    IEnumerable<ExchangeRate> rates = await rateService.GetAllCurrentRatesAsync();
    if(!rates.Any()) {
        return Results.Ok(new { Status = "Initializing", Message = "Rates are being fetched for the first time. Please check back in a moment." });
    }

    Dictionary<string, decimal> response = rates.ToDictionary(r => r.Pair.ToString(), r => r.Value.Value);

    return Results.Ok(new { Rates = response });
})
.WithName("GetServiceStatus")
.WithTags("Health");

app.MapGet("/api/rates/{fromCurrency}/{toCurrency}",
    async (string fromCurrency, string toCurrency, IExchangeRateService rateService) => {
        try {
            if(string.Equals(fromCurrency, toCurrency, StringComparison.OrdinalIgnoreCase))
                return Results.Ok(1.0m);

            ExchangeRate? rate = await rateService.GetRateAsync(Currency.New(fromCurrency), Currency.New(toCurrency));

            return rate is not null
                ? Results.Ok(rate.Value.Value)
                : Results.NotFound(new { Message = $"Exchange rate from {fromCurrency} to {toCurrency} not found." });
        }
        catch(ArgumentException ex) {
            return Results.BadRequest(new { ex.Message });
        }
    })
.WithName("GetExchangeRate")
.WithTags("Rates")
.Produces<decimal>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest);


RecurringJob.AddOrUpdate<ExchangeRateFetcherJob>(
    "fetch-tcmb-rates",
    job => job.ExecuteAsync(),
    Cron.Hourly);

await app.RunAsync(default(CancellationToken));

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));