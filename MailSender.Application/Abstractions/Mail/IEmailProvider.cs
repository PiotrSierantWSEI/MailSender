namespace MailSender.Application.Abstractions.Mail;
// Interfejs definiujacy providera odpowiedzialnego za wysylke maili.
public interface IEmailProvider
{
    // Zwraca techniczny wynik wysylki, zeby use case mogl zdecydowac o statusie operacji.
    Task<EmailSendResult> SendEmailAsync(
        EmailMessage emailMessage,
        CancellationToken cancellationToken);
}