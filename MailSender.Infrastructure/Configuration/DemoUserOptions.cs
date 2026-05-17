namespace MailSender.Infrastructure.Configuration;

// Parametry potrzebne do tworzenia i walidowania usera Demo 
public sealed class DemoUserOptions
{
    // Nazwa sekcji konfiguracji w pliku appsettings.json
    public const string SectionName = "AuthDemoUser";

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
