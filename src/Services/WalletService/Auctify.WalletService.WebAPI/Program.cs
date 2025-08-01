using Auctify.WalletService.Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddWalletApplicationServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
 
app.Run(); 