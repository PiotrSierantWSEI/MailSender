// przyjmujemy dane
namespace MailSender.Application.MailSend;

public sealed record MailSendCommand(
    string? AppId,
    string? AppName,
    string To,
    string Subject,
    string Body
);