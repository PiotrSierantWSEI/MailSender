namespace MailSender.Application.Abstractions.Mail;

public sealed record EmailSendResult(
    bool Success,
    string? ProviderMessageId = null,
    string? Error = null);