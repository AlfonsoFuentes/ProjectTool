using Application;
using Application.Interfaces;
using Domain.Entities.Account;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Commons.UserManagement;
using System.Text;
namespace Server.Services
{
    public static class RegisterServices
    {
        public static WebApplicationBuilder AddServerServices2(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            // cookie authentication
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

            // configure authorization
            builder.Services.AddAuthorizationBuilder();

            var cs = builder.Configuration.GetConnectionString("Default");
            // add the database (in memory for the sample)
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(cs));

            // add identity and opt-in to endpoints
            builder.Services.AddIdentityCore<AplicationUser>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 5;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.ClaimsIdentity.UserIdClaimType = "ProjectToolId";
            })
                 .AddSignInManager()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();
            var frontend = builder.Configuration["FrontendUrl"];
            var backend = builder.Configuration["BackendUrl"];
            // add CORS policy for Wasm client
            //builder.Services.AddCors(
            //    options => options.AddPolicy(
            //        "wasm",
            //        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:5001",
            //            builder.Configuration["FrontendUrl"] ?? "https://localhost:5002"])
            //            .AllowAnyMethod()
            //            .SetIsOriginAllowed(pol => true)
            //            .AllowAnyHeader()
            //            .AllowCredentials()));

            builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at
            // https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<CurrentUser>();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            return builder;
        }
        public static IServiceCollection AddServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddDbContext<AppDbContext>(options =>

                  options.UseSqlServer(configuration.GetConnectionString("Default"),
                     b => b.MigrationsAssembly(typeof(RegisterServices).Assembly.FullName)),
                     ServiceLifetime.Scoped
             );

            //Add Identity & JWT authentication
            //Identity
            services.AddIdentity<AplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddRoles<IdentityRole>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };

            });
            services.AddSingleton<CurrentUser>();
            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.AddApplication();
            services.AddInfrastructure();
            return services;
        }
    }
}
