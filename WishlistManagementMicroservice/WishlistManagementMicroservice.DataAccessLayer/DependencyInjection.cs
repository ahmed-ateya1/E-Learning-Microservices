using Microsoft.Extensions.DependencyInjection;
using WishlistManagementMicroservice.DataAccessLayer.Data;
using WishlistManagementMicroservice.DataAccessLayer.RepoistoryContract;
using WishlistManagementMicroservice.DataAccessLayer.Repositories;

namespace WishlistManagementMicroservice.DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<DapperDbContext>();
            return services;
        }
    }
}
