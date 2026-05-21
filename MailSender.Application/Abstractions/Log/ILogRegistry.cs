using MailSender.Domain.Log;

namespace MailSender.Application.Abstractions.Log;

// Interfejs rejestru logów
public interface ILogRegistry
{
    IReadOnlyList<LogIdentity> GetAllClientApps();
    IReadOnlyList<LogIdentity> GetAllClientAppsByAppId(string appId);
    IReadOnlyList<LogIdentity> GetAllClientAppsByAppName(string appName);
    void RegisterLog(LogIdentity log);
}
