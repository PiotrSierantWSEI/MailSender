using System.Text;
using MailSender.Application.Abstractions.Auth;
using MailSender.Application.Abstractions.ClientApp;
using MailSender.Infrastructure.Authentication;
using MailSender.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailSender.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Konfiguracja JWT 
        services
            .AddOptions<JwtOptions>()
            // Pobiera sekcję konfiguracji z pliku appsettings.json i mapuje wartości do obiektu JwtOptions.
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            // Walidacja sprawdza czy wszystkie wartości spełniają wymagania
            .Validate(
                options =>
                    !string.IsNullOrWhiteSpace(options.Key) &&
                    Encoding.UTF8.GetByteCount(options.Key) >= 32 &&
                    !string.IsNullOrWhiteSpace(options.Issuer) &&
                    !string.IsNullOrWhiteSpace(options.Audience) &&
                    options.ExpiresInMinutes > 0,
                "Jwt configuration is invalid. Set Jwt:Key to at least 32 UTF-8 bytes and configure Issuer, Audience, and ExpiresInMinutes.")
            // Wymusza walidację przy starcie aplikacji 
            .ValidateOnStart();

        // Konfiguracja użytkownika demo
        services
            .AddOptions<DemoUserOptions>()
            // tak samo z sekcją AuthDemoUser z pliku appsettings.json
            .Bind(configuration.GetSection(DemoUserOptions.SectionName))
            .Validate(
                options =>
                    !string.IsNullOrWhiteSpace(options.Username) &&
                    !string.IsNullOrWhiteSpace(options.Password),
                "AuthDemoUser configuration is invalid. Set AuthDemoUser:Username and AuthDemoUser:Password.")
            .ValidateOnStart();

        // Konfiguracja aplikacji klienckich
        services
            .AddOptions<ClientAppsOptions>()
            // tak samo z sekcją ClientApps z pliku appsettings.json
            .Bind(configuration.GetSection(ClientAppsOptions.SectionName))
            .Validate(
                options => options.Passwords != null && options.Passwords.Length > 0,
                "ClientApps configuration is invalid. Set ClientApps:Passwords to a non-empty array.")
            .ValidateOnStart();

        // Konfiguracja polityk autoryzacji - potrzebna do zabezpieczenia np. do wyslania maila.
        services
            .AddAuthorizationBuilder()
            .AddPolicy("ClientApp", policy => policy.RequireClaim("app_id"));

        // Konfiguracja usług związanych z uwierzytelnianiem
        services.AddScoped<ICredentialValidator, ConfigurationCredentialValidator>();
        services.AddScoped<IAccessTokenIssuer, JwtAccessTokenIssuer>();
        services.AddScoped<IClientAppAccessTokenIssuer, ClientAppAccessTokenIssuer>();
        services.AddScoped<IClientAppPasswordValidator, ClientAppPasswordValidator>();

        return services;
    }
}
