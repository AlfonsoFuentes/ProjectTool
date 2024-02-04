using Application.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
      
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IMWORepository, MWORepository>();
            services.AddScoped<IBudgetItemRepository, BudgetItemRepository>();
  
            return services;
        }

    }
}
