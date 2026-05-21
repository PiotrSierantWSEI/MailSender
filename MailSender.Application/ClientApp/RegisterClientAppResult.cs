namespace MailSender.Application.ClientApp;

public sealed record RegisterClientAppResult(
    string? Key,
    DateTime? ExpiresAtUtc,
    RegisterClientAppError Error
);

public enum RegisterClientAppError
{
    None,
    InvalidInput,
    InvalidPassword,
    AppAlreadyExists,
}
