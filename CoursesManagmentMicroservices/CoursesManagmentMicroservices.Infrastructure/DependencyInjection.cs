using CoursesManagmentMicroservices.Core.Domain.RepositoryContract;
using CoursesManagmentMicroservices.Core.MappingProfile.CategoryProfile;
using CoursesManagmentMicroservices.Core.ServiceContract;
using CoursesManagmentMicroservices.Core.Services;
using CoursesManagmentMicroservices.Core.Validators.CategoryValidators;
using CoursesManagmentMicroservices.Infrastructure.Data;
using CoursesManagmentMicroservices.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
            string connectionString = connectionStringTemplate
                .Replace("$MSSQL_SERVER", Environment.GetEnvironmentVariable("MSSQL_SERVER"))
                .Replace("$MSSQL_DB", Environment.GetEnvironmentVariable("MSSQL_DB"))
                .Replace("$MSSQL_PASSWORD", Environment.GetEnvironmentVariable("MSSQL_PASSWORD"));

            options.UseSqlServer(connectionString);
        });

        // Register AutoMapper
        services.AddAutoMapper(typeof(CategoryMappingProfile).Assembly);

        // Register Validators
        services.AddValidatorsFromAssemblyContaining<CategoryAddRequestValidator>();

        // Register Services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}