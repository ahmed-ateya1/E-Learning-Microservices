using CoursesManagmentMicroservices.Core.MappingProfile.CategoryProfile;
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
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CategoryMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<CategoryAddRequestValidator>();
            services.AddScoped<ICategoryService, CategoryService>();
            return services;
        }
    }
}
