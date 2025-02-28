using CartManagementMicroservice.BusinessLayer;
using Microsoft.OpenApi.Models;

namespace CartManagementMicroservice.API.StartupExtensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection ServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddHealthChecks();
            services.AddLogging();
            services.AddControllers();
            services.AddBusinessLayer();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping Cart Managment Microserices", Version = "v1" });
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
