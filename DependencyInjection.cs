using System.Security.Claims;
using System.Text;
using MailSender.Application.Auth;
using MailSender.Application.MailSend;
using MailSender.Application.ClientApp;
using MailSender.Application.Log;
using MailSender.Infrastructure;
using MailSender.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace MailSender;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Rejestracja use case'ów.
        services.AddScoped<AuthenticateUserUseCase>();
        services.AddScoped<RegisterClientAppUseCase>();
        services.AddScoped<MailSendAppUseCase>();
        services.AddScoped<LogUseCase>();
        // Rejestracja infrastruktury - tutaj dodajemy wszystkie zależności z warstwy infrastruktury.
        services.AddInfrastructure(configuration);

        // Polityka CORS - pozwalamy na dostęp do API z dowolnego źródła (w praktyce warto to ograniczyć do konkretnych domen).
        // https://wseii-my.sharepoint.com/:p:/g/personal/csiwon_wsei_edu_pl/IQALPvZ8jiHdR5VHKOjkH91HAR8hf_7QsIAfjourOgXV1-Y?e=WbDAty
        // Slajd 251 z wykładów.
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // w pliku appsetings.json sa umieszczone dane zetonu (wydawca i klientow)
                var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                    ?? throw new InvalidOperationException("Jwt configuration is missing.");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // konfiguracja walidacji tokena JWT 
                    // https://wseii-my.sharepoint.com/:p:/g/personal/csiwon_wsei_edu_pl/IQALPvZ8jiHdR5VHKOjkH91HAR8hf_7QsIAfjourOgXV1-Y?e=WbDAty
                    // 234 slajd z wykladów
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    NameClaimType = ClaimTypes.Name,
                    // token jest ważny dokładnie w swoim przedziale czasu - ustawiamy tolerancje czasową na zero
                    ClockSkew = TimeSpan.Zero
                };
            });

        // Rejestracja autoryzacji, która wymaga uwierzytelnienia za pomocą JWT.
        services.AddAuthorization();
        // Rejestracja Swagger/OpenAPI do dokumentacji API.
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Wklej token w formacie: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
