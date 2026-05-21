using System.Text;
using MailSender.Application.Abstractions.Auth;
using MailSender.Application.Abstractions.ClientApp;
using MailSender.Application.Abstractions.Mail;
using MailSender.Application.Abstractions.Log;
using MailSender.Infrastructure.Authentication;
using MailSender.Infrastructure.ClientApp;
using MailSender.Infrastructure.Configuration;
using MailSender.Infrastructure.Mail;
using MailSender.Infrastructure.LogRegistry;

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

        // dodanie zmiennych z appsettings.json do konteneru DI, zeby mozna bylo je wstrzykiwac do providerów mailowych
        services
            .AddOptions<BrevoOptions>()
            .Bind(configuration.GetSection(BrevoOptions.SectionName))
            .Validate(
                options =>
                    !string.IsNullOrWhiteSpace(options.SmtpHost) &&
                    options.SmtpPort > 0 &&
                    !string.IsNullOrWhiteSpace(options.SmtpLogin) &&
                    !string.IsNullOrWhiteSpace(options.SmtpPassword) &&
                    !string.IsNullOrWhiteSpace(options.SenderEmail),
                "Brevo configuration is invalid.")
            .ValidateOnStart();

        services
            .AddOptions<MailTrapOptions>()
            .Bind(configuration.GetSection(MailTrapOptions.SectionName))
            .Validate(
                options =>
                    !string.IsNullOrWhiteSpace(options.SmtpHost) &&
                    options.SmtpPort > 0 &&
                    !string.IsNullOrWhiteSpace(options.SmtpLogin) &&
                    !string.IsNullOrWhiteSpace(options.SmtpPassword) &&
                    !string.IsNullOrWhiteSpace(options.SenderEmail),
                "MailTrap configuration is invalid.")
            .ValidateOnStart();

        // Dependency Injection
        services.AddScoped<ICredentialValidator, ConfigurationCredentialValidator>();
        services.AddScoped<IAccessTokenIssuer, JwtAccessTokenIssuer>();
        services.AddScoped<IClientAppAccessTokenIssuer, ClientAppAccessTokenIssuer>();
        services.AddScoped<IClientAppPasswordValidator, ClientAppPasswordValidator>();


        // Singleton ponieważ chcemy mieć w pamięci - jesli uzyjemy scoped to przy kazdym requestcie bedzie tworzona nowa instancja
        services.AddSingleton<IClientAppRegistry, ClientAppRegistryInMemory>();

        // DI providerów
        services.AddScoped<IEmailProvider, BrevoEmailProvider>();
        // services.AddScoped<IEmailProvider, MailTrapEmailProvider>();
        // Zmiana providera mailowego odbywa sie przez podmiane implementacji IEmailProvidera.
        // services.AddScoped<IEmailProvider, <inny provider>>();
        services.AddSingleton<ILogRegistry, LogRegistryInMemory>();
        return services;
    }
}
