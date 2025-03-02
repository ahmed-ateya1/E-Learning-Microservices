
using FluentValidation.AspNetCore;
using WishlistManagementMicroservice.API.ApiEndpoints;
using WishlistManagementMicroservice.API.Middleware;
using WishlistManagementMicroservice.BusinessLayer;

namespace WishlistManagementMicroservice.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddBusinessLayer(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            app.UseExceptionHandlingMiddleware();
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapWishlistEndpoints();

            app.Run();
        }
    }
}
