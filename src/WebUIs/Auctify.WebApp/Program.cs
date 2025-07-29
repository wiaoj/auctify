using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Auctify.WebApp;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//await builder.Build().RunAsync();


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// AuctionService API'si için isimlendirilmiþ bir HttpClient ekliyoruz.
// Adres, AuctionService'inizin çalýþtýðý adres olmalý.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7164") });
 

await builder.Build().RunAsync();