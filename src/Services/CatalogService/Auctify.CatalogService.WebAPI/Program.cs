WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();



await app.RunAsync(default(CancellationToken));