namespace MailSender.Application.Auth;

public sealed record AuthenticateUserCommand(string Username, string Password);
