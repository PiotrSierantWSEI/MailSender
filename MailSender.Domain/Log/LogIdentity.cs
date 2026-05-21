namespace MailSender.Domain.LogRegistry;

// Reprezentuje tożsamość logu,
public sealed record LogIdentity(string Id, string Status, string AppId, string AppName, string? Message, DateTime Timestamp);
