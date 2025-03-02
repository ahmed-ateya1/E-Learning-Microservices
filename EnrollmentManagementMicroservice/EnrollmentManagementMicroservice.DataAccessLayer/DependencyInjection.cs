using EnrollmentManagementMicroservice.DataAccessLayer.Data;
using EnrollmentManagementMicroservice.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnrollmentManagementMicroservice.DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
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
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            return services;
        }
    }
}
