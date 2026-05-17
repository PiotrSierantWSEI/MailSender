namespace MailSender.Application.Auth;

public sealed record AuthenticateUserResult(string AccessToken, DateTime ExpiresAtUtc);
