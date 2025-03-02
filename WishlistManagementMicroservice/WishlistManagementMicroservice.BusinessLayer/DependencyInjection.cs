using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WishlistManagementMicroservice.BusinessLayer.Dtos;
using WishlistManagementMicroservice.BusinessLayer.HttpClients;
using WishlistManagementMicroservice.BusinessLayer.MappingProfile;
using WishlistManagementMicroservice.BusinessLayer.ServiceContract;
using WishlistManagementMicroservice.BusinessLayer.Services;
using WishlistManagementMicroservice.BusinessLayer.Validators;
using WishlistManagementMicroservice.DataAccessLayer;

namespace WishlistManagementMicroservice.BusinessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDataAccessLayer();
            services.AddAutoMapper(typeof(WishlistMappingProfile).Assembly);
            services.AddTransient<IValidator<WishlistAddRequest>, WishlistAddRequestValidator>();

            services.AddDistributedMemoryCache();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IUserMicroserviceHttpClient, UserMicroserviceHttpClient>();
            services.AddScoped<ICourseMicroserviceHttpClient, CourseMicroserviceHttpClient>();
            services.AddValidatorsFromAssemblyContaining<WishlistAddRequestValidator>();
            services.AddHttpClient<IUserMicroserviceHttpClient, UserMicroserviceHttpClient>(options =>
            {
                options.BaseAddress = new Uri($"http://{configuration["UserMicroserviceName"]}:{configuration["userMicroservicePort"]}");
            });
            services.AddHttpClient<ICourseMicroserviceHttpClient, CourseMicroserviceHttpClient>(options =>
            {
                options.BaseAddress = new Uri($"http://{configuration["CourseMicroserviceName"]}:{configuration["CourseMicroservicePort"]}");
            });
            return services;
        }
    }
}
