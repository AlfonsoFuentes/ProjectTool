
using Blazored.LocalStorage;

using System.Globalization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Client.Infrastructure.RegisterServices
{
    public static class RegisterServices
    {
        private const string ClientName = "Auth";
        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<RefreshTokenService>();
  
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


            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddScoped<IHttpClientService, HttpClientService>();

            builder.Services.AddScoped<HttpInterceptorService>();
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
