namespace MailSender.Domain.ClientApp;

// Reprezentuje tożsamość aplikacji klienckiej, zawierającą jej unikalny identyfikator (AppId) oraz nazwę (AppName).
public sealed record ClientAppIdentity(string AppId, string AppName);
