namespace MailSender.Infrastructure.Configuration;

// Parametry potrzebne do tworzenia i walidowania tokenów
public sealed class JwtOptions
{
    // Nazwa sekcji konfiguracji w pliku appsettings.json
    public const string SectionName = "Jwt";

    public string Key { get; init; } = string.Empty;

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public int ExpiresInMinutes { get; init; } = 60;
}
