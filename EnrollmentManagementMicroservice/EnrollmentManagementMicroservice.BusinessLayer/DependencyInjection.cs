using EnrollmentManagementMicroservice.BusinessLayer.HttpClients;
using EnrollmentManagementMicroservice.BusinessLayer.MappingProfile;
using EnrollmentManagementMicroservice.BusinessLayer.Services;
using EnrollmentManagementMicroservice.BusinessLayer.Validators;
using EnrollmentManagementMicroservice.DataAccessLayer;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnrollmentManagementMicroservice.BusinessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDataAccessLayer(configuration);
            services.AddDistributedMemoryCache();
            services.AddAutoMapper(typeof(EnrollmentMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<EnrollmentAddRequestValidator>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
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
