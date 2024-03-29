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
      
        public static WebApplicationBuilder AddServerServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            // cookie authentication
           

        
            builder.Services.AddSingleton<CurrentUser>();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            return builder;
        }

    }
}
