namespace MailSender.Domain.Log;

// Reprezentuje tożsamość logu,
public sealed record LogIdentity(string Status, string AppId, string AppName, string? Message, DateTime Timestamp);
