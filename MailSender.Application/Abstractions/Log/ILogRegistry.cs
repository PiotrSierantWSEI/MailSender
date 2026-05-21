using MailSender.Domain.LogRegistry;

namespace MailSender.Application.Abstractions.Log;

// Interfejs rejestru logów
public interface ILogRegistry
{
    IReadOnlyList<LogIdentity> GetAllClientApps();
    IReadOnlyList<LogIdentity> GetAllClientAppsByAppId(string appId);
    void RegisterLog(LogIdentity log);
}
