using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizManagementMicroservice.Core.Caching;
using QuizManagementMicroservice.Core.HttpClients;
using QuizManagementMicroservice.Core.MappingProfile;
using QuizManagementMicroservice.Core.ServiceContracts;
using QuizManagementMicroservice.Core.Services;
using QuizManagementMicroservice.Core.Validators;

namespace QuizManagementMicroservice.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
            services.AddScoped<ICacheService , CacheService>();
            services.AddScoped<IQuizzService, QuizzService>();
            services.AddAutoMapper(typeof(QuizzMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<QuizzAddRequestValidator>();
            services.AddHttpClient<IUserMicroserviceClient , UserMicroserviceClient>(options =>
            {
                options.BaseAddress = new Uri($"http://{configuration["UserMicroserviceName"]}:{configuration["userMicroservicePort"]}");
            });
            services.AddHttpClient<ILectureMicroserviceClient, LectureMicroserviceClient>(options =>
            {
                options.BaseAddress = new Uri($"http://{configuration["LectureMicroserviceName"]}:{configuration["lectureMicroservicePort"]}");
            });

            return services;
        }
    }
}
