namespace MailSender.Application.Abstractions.Mail;

public sealed record EmailMessage(
    string To,
    string Subject,
    string Body,
    string? From = null);