namespace MailSender.Infrastructure.Configuration;

// Parametry potrzebne do konfiguracji polaczenia z MailTrap
public sealed class MailTrapOptions
{
    // Nazwa sekcji konfiguracji w pliku appsettings.json
    public const string SectionName = "MailTrap";

    public string SmtpHost { get; set; } = null!;
    public int SmtpPort { get; set; }
    public string SmtpLogin { get; set; } = null!;
    public string SmtpPassword { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string SenderName { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
}
