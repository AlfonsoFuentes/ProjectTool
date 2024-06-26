﻿using Application;
using Infrastructure;
using Infrastructure.Services;
namespace Server.Services
{
    public static class RegisterServices
    {
      
        public static WebApplicationBuilder AddServerServices(this WebApplicationBuilder builder)
        {
           
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<ITenantService, TenantService>();
            builder.Services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"));
            });
            builder.Services.AddScoped<ITokenService, TokenService>();
            
            return builder;
        }

    }
}
