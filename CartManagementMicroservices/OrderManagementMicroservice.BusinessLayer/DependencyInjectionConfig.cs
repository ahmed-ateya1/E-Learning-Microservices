using CartManagementMicroservice.BusinessLayer.ServiceContract;
using CartManagementMicroservice.BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CartManagementMicroservice.BusinessLayer
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();
            services.AddMemoryCache();
            return services;
        }
    }
}
