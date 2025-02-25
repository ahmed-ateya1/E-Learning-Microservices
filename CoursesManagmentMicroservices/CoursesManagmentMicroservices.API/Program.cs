using CoursesManagmentMicroservices.API.Middleware;
using CoursesManagmentMicroservices.Core;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace CoursesManagmentMicroservices.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddCore();
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddLogging();
            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Courses Managment Microserices", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
            });
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            builder.Services.AddOpenApi();
            var app = builder.Build();

            app.UseExceptionHandlingMiddleware();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }

    }
}
