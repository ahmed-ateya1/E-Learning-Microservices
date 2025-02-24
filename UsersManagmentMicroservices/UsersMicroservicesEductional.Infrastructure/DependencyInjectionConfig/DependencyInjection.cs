using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsersMicroservicesEductional.Core.Domain.IdentityEntities;
using UsersMicroservicesEductional.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersMicroservicesEductional.Core.Dtos.AuthenticationDto;
using UsersMicroservicesEductional.Core.Domain.RepositoryContract;
using UsersMicroservicesEductional.Infrastructure.Repositories;

namespace UsersMicroservicesEductional.Infrastructure.DependencyInjectionConfig
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddScoped<IUnitOfWork,UnitOfWork>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
                string connectionString = connectionStringTemplate
                    .Replace("$SERVER_NAME", Environment.GetEnvironmentVariable("SERVER_NAME"))
                    .Replace("$DATABASE_NAME", Environment.GetEnvironmentVariable("DATABASE_NAME"))
                    .Replace("$USER_SQLSERVER", Environment.GetEnvironmentVariable("USER_SQLSERVER"))
                    .Replace("$PASSWORD_SQLSERVER", Environment.GetEnvironmentVariable("PASSWORD_SQLSERVER"));
                options.UseSqlServer(connectionString);
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 5;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders()
              .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
              .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };

                // Add custom JwtBearerEvents
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Handle token extraction from headers
                        context.Token = context.Request.Headers["Authorization"]
                            .FirstOrDefault()?.Split(" ").Last();
                        return Task.CompletedTask;
                    }
                };
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000", builder =>
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .SetIsOriginAllowed(origin => true)
                           .WithExposedHeaders("Set-Cookie"));
            });
            services.Configure<JwtDTO>(configuration.GetSection("JWT"));

            return services;
        }
    }
}
