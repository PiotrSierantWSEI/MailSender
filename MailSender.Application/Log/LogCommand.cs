namespace MailSender.Application.Log;

public sealed record LogCommand(
    string? AppId,
    string? LogId
);