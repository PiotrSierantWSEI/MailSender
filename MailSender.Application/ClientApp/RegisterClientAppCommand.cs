namespace MailSender.Application.ClientApp;

public sealed record RegisterClientAppCommand(
    string AppId,
    string AppName,
    string Pass);
