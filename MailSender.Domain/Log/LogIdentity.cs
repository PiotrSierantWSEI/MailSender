namespace MailSender.Domain.Log;

// Reprezentuje tożsamość logu,
public sealed record LogIdentity(string status, string appId, string appName, string? message, DateTime timestamp);
