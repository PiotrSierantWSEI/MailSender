namespace MailSender.Contracts.ClientApp;

public sealed record RegisterClientAppResponse(
    string AppId,
    string AppName,
    string Key);
