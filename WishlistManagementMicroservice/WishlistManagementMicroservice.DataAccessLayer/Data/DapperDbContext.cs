using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace WishlistManagementMicroservice.DataAccessLayer.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;

        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection DbConnection
        {
            get
            {
                string connectionStringTemplate
                    = _configuration.GetConnectionString("DefaultConnection")!;

                string connectionString =
                    connectionStringTemplate
                    .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost")
                    .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432")
                    .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? "wishlistdb")
                    .Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres")
                    .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "ahmed");

                return new NpgsqlConnection(connectionString);
            }
        }
    }
}