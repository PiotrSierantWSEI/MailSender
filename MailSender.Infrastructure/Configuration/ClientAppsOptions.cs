namespace MailSender.Infrastructure.Configuration;

// Parametry potrzebne do tworzenia i walidowania aplikacji klienckich
public sealed class ClientAppsOptions
{
    // Nazwa sekcji konfiguracji w pliku appsettings.json
    public const string SectionName = "ClientApps";

    public string[] Passwords { get; init; } = [];
}
