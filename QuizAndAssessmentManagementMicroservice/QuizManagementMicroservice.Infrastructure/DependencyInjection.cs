using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizManagementMicroservice.Core.Domain.RepositoryContracts;
using QuizManagementMicroservice.Infrastructure.Data;
using QuizManagementMicroservice.Infrastructure.Repositories;

namespace QuizManagementMicroservice.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
                string connectionString = connectionStringTemplate
                    .Replace("$MSSQL_SERVER", Environment.GetEnvironmentVariable("MSSQL_SERVER")) 
                    .Replace("$MSSQL_DB", Environment.GetEnvironmentVariable("MSSQL_DB"))
                    .Replace("$MSSQL_PASSWORD", Environment.GetEnvironmentVariable("MSSQL_PASSWORD"));

                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           
            return services;
        }
    }
}
