using CoursesManagmentMicroservices.API.FileServiceContract;
using CoursesManagmentMicroservices.API.Middleware;
using CoursesManagmentMicroservices.API.ServiceConfiguration;
using CoursesManagmentMicroservices.Core;
using CoursesManagmentMicroservices.Core.ServiceContract;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace CoursesManagmentMicroservices.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMainServices(builder.Configuration);
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
