namespace MailSender.Application.ClientApp;

public sealed record RegisterClientAppResult(
    string Key,
    DateTime ExpiresAtUtc);
