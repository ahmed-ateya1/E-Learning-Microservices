using CoursesManagmentMicroservices.API.FileServiceContract;
using CoursesManagmentMicroservices.Core;
using CoursesManagmentMicroservices.Core.ServiceContract;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace CoursesManagmentMicroservices.API.ServiceConfiguration
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddMainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IFileServices, FileService>();
            services.AddInfrastructure(configuration);
            services.AddCore(configuration);
            services.AddControllers();
            services.AddHealthChecks();
            services.AddLogging();
            services.AddControllers();
            services.AddFluentValidationAutoValidation();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Courses Managment Microserices", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            services.AddOpenApi();

            return services;

        }
    }
}
