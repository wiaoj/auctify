using Auctify.WebApp;
using Auctify.WebApp.Services; 
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("BidService", client => {
    client.BaseAddress = new Uri("https://localhost:7239");
});
builder.Services.AddHttpClient("CatalogService", client => {
    client.BaseAddress = new Uri("https://localhost:7097");
});

builder.Services.AddScoped<AuctionHubService>();

await builder.Build().RunAsync(); 