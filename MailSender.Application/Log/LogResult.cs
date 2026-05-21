using MailSender.Domain.LogRegistry;
namespace MailSender.Application.Log;

public sealed record LogsResult(
    List<LogIdentity>? Logs,
    ErrorResult? Error
);

public sealed record ErrorResult(
    string ErrorMessage,
    ErrorType Type
);

public enum ErrorType
{
    NotFound,
    BadRequest,
}