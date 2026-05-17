namespace MailSender.Contracts.Auth;

public sealed record IssueTokenRequest(string Username, string Password);
