namespace MailSender.Contracts.Auth;

public sealed record IssueTokenResponse(string AccessToken, DateTime ExpiresAtUtc);
