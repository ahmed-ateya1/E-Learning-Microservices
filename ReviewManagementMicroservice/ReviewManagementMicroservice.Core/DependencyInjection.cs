using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReviewManagementMicroservice.Core.HttpClients;
using ReviewManagementMicroservice.Core.MappingProfile;
using ReviewManagementMicroservice.Core.ServiceContract;
using ReviewManagementMicroservice.Core.Services;
using ReviewManagementMicroservice.Core.Validators;

namespace ReviewManagementMicroservice.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
            services.AddAutoMapper(typeof(ReviewMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<ReviewAddRequestValidator>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddHttpClient<IUserMicroserviceClient, UserMicroserviceClient>(options =>
            {
                options.BaseAddress = new Uri($"http://{configuration["UserMicroserviceName"]}:{configuration["userMicroservicePort"]}");
            });
            services.AddHttpClient<ICourseMicroserviceClient, CourseMicroserviceClient>(options =>
            {
                options.BaseAddress = new Uri($"http://{configuration["CourseMicroserviceName"]}:{configuration["CourseMicroservicePort"]}");
            });
            return services;
        }
    }
}
