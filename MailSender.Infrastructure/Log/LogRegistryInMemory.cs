using MailSender.Domain.Log;
using MailSender.Application.Abstractions.Log;

namespace MailSender.Infrastructure.LogRegistry;

public class LogRegistryInMemory : ILogRegistry
{
    private readonly List<LogIdentity> _logs = [];

    public IReadOnlyList<LogIdentity> GetAllClientApps() => _logs.AsReadOnly();


    public IReadOnlyList<LogIdentity> GetAllClientAppsByAppId(string appId) => _logs.Where(log => log.AppId == appId).ToList().AsReadOnly();

    public IReadOnlyList<LogIdentity> GetAllClientAppsByAppName(string appName) => _logs.Where(log => log.AppName == appName).ToList().AsReadOnly();

    public void RegisterLog(LogIdentity log)
    {
        // dodanie logow nie powinno przerywac dzialania apki
        _logs.Add(log);
    }
}