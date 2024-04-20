using BlazorDownloadFile;
using Client.Infrastructure.RegisterServices;
using ClientRadzen;
using ClientRadzen.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<NavigationBack>();
builder.AddClientServices();
builder.Services.AddRadzenComponents();
builder.Services.AddBlazorDownloadFile();
var host = builder.Build();
host.Services.GetService<NavigationBack>();
await host.RunAsync();

