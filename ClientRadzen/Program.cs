using ClientRadzen.RegisterServices;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.AddClientServices();
builder.Services.AddRadzenComponents();
await builder.Build().RunAsync();
