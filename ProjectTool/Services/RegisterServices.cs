using Application;
using Infrastructure;
using Shared.Commons.Results;
namespace Server.Services
{
    public static class RegisterServices
    {
      
        public static WebApplicationBuilder AddServerServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<CurrentUser>();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            return builder;
        }

    }
}
