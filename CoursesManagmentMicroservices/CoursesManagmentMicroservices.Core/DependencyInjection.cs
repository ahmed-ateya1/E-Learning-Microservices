using CoursesManagmentMicroservices.Core.Caching;
using CoursesManagmentMicroservices.Core.HttpClients;
using CoursesManagmentMicroservices.Core.MappingProfile.CategoryProfile;
using CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries;
using CoursesManagmentMicroservices.Core.ServiceContract;
using CoursesManagmentMicroservices.Core.Services;
using CoursesManagmentMicroservices.Core.Validators.CategoryValidators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesManagmentMicroservices.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(CategoryMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<CategoryAddRequestValidator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllCoursesQuery).Assembly));

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILectureService, LectureService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUserMicroserviceClient, UserMicroserviceClient>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISectionServices, SectionServices>();
            services.AddHttpClient<IUserMicroserviceClient, UserMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri($"http://{configuration["UserMicroserviceName"]}:{configuration["userMicroservicePort"]}");
            });
            services.AddDistributedMemoryCache();
            return services;
        }
    }
}
