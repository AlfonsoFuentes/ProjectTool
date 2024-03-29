﻿
using Client.Infrastructure.Managers.CurrencyApis;
using System.Globalization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Client.Infrastructure.RegisterServices
{
    public static class RegisterServices
    {
        private const string ClientName = "Auth";
        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {

           


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

            var frontend = builder.Configuration["FrontendUrl"];
            var backend = builder.Configuration["BackendUrl"];
            // set base address for default host

            builder.Services.AddScoped(sp => sp
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient(ClientName).EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                });

            // builder.Services.AddHttpClient("Auth", client =>
            //client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            //      .AddHttpMessageHandler<CookieHandler>(); ;


            // // Supply HttpClient instances that include access tokens when making requests to the server project
            // builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Auth"));

            //builder.Services.AddScoped(sp =>
            //    new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:5002") });

            //// configure client for auth interactions
            //builder.Services.AddHttpClient(
            //    "Auth",
            //    opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001"))
            //    .AddHttpMessageHandler<CookieHandler>();
            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddOptions();
            builder.Services.AddManagers();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.CurrencyService();
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
