namespace MailSender.Contracts.ClientApp;

public sealed record RegisterClientAppRequest(
    string AppId,
    string AppName,
    string Pass);
