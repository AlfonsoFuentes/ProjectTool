﻿using Application.Interfaces;
using Domain.Entities.Account;
using Infrastructure.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Suppliers;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>

                options.UseSqlServer(configuration.GetConnectionString("Default"),
                   b => b.MigrationsAssembly(typeof(RegisterServices).Assembly.FullName)),
                   ServiceLifetime.Scoped
           );

           
            services.AddIdentity<AplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddRoles<IdentityRole>();

            var frontend = configuration["FrontendUrl"];
            var backend = configuration["BackendUrl"];
           
           
            var jwtSettings = configuration.GetSection("JWTSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]!))
                };
            });

            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IMWORepository, MWORepository>();
            services.AddScoped<IBudgetItemRepository, BudgetItemRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderValidatorRepository, PurchaseOrderValidatorRepository>();
     
          
            services.AddScoped<ISapAdjustRepository, SapAdjustRepository>();
            services.AddScoped<IExcelService, ExcelService>();
     
            return services;
        }
      
    }
}
