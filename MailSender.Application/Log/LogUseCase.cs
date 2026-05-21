using MailSender.Application.Abstractions.Log;
using MailSender.Domain.LogRegistry;

namespace MailSender.Application.Log;

public sealed class LogUseCase(ILogRegistry logRegistry)
{
    public Task<LogsResult> Execute(
        LogCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.AppId))
        {
            return Task.FromResult(new LogsResult(null, new ErrorResult("AppId is required", ErrorType.BadRequest)));
        }

        var logs = logRegistry.GetAllClientAppsByAppId(command.AppId).ToList();

        if (!string.IsNullOrWhiteSpace(command.LogId))
        {
            logs = logs.Where(log => log.Id == command.LogId).ToList();
            if (logs.Count == 0)
            {
                return Task.FromResult(new LogsResult(null, new ErrorResult("Log not found", ErrorType.NotFound)));
            }
            return Task.FromResult(new LogsResult(logs, null));
        }

        return Task.FromResult(new LogsResult(logs, null));
    }
}