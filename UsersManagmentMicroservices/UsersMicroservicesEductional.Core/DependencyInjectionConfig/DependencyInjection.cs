using FluentValidation;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using UsersMicroservicesEductional.Core.Dtos.AuthenticationDto;
using UsersMicroservicesEductional.Core.ServiceContract;
using UsersMicroservicesEductional.Core.Services;

namespace UsersMicroservicesEductional.Core.DependencyInjectionConfig
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<LoginDTO>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }
    }
}
