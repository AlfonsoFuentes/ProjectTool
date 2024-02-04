using Client.Identity;
using Client.Managers;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Commons.UserManagement;
using System.Reflection;

namespace Client.Services
{
    public static class WebAssemblyHostBuilderExtensions
    {   public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");


            // register the cookie handler
            builder.Services.AddScoped<CookieHandler>();

            // set up authorization
            builder.Services.AddAuthorizationCore();

            // register the custom state provider
            builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
            builder.Services.AddScoped<CurrentUser>();
            // register the account management interface
            builder.Services.AddScoped(
                sp => (ICookieAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());

            //var baseadress = builder.HostEnvironment.BaseAddress;
            //builder.Services.AddHttpClient("CP.PM.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            //    .AddHttpMessageHandler<CookieHandler>();
            //builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CP.PM.ServerAPI"));

            var frontend = builder.Configuration["FrontendUrl"];
            var backend = builder.Configuration["BackendUrl"];
            // set base address for default host
            builder.Services.AddScoped(sp =>
                new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:5002") });

            // configure client for auth interactions
            builder.Services.AddHttpClient(
                "Auth",
                opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001"))
                .AddHttpMessageHandler<CookieHandler>();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddManagers();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
       
            return builder;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            var managers = typeof(IManager);

            var types = managers
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null).ToList();

            foreach (var type in types)
            {
                if (managers.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
            }

            return services;
        }
    }
}
