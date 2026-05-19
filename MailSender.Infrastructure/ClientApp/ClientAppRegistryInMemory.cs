using MailSender.Domain.ClientApp;
using MailSender.Application.Abstractions.ClientApp;

namespace MailSender.Infrastructure.ClientApp;

// Trzymamy dane o aplikacjach w pamięci
public class ClientAppRegistryInMemory : IClientAppRegistry
{
    // Implementacja interface'u
    // IReadOnlyList<ClientAppIdentity> GetAllRegisteredApps();
    // ClientAppIdentity? GetRegisteredAppById(string appId);
    // ClientAppIdentity? GetRegisteredAppByName(string appName);
    // void RegisterClientApp(ClientAppIdentity clientApp);

    private readonly List<ClientAppIdentity> _registeredApps = [];
    public IReadOnlyList<ClientAppIdentity> GetAllRegisteredApps() => _registeredApps.AsReadOnly();
    public ClientAppIdentity? GetRegisteredAppById(string appId) => _registeredApps.Find(app => app.AppId == appId);
    public ClientAppIdentity? GetRegisteredAppByName(string appName) => _registeredApps.Find(app => app.AppName == appName);
    public void RegisterClientApp(ClientAppIdentity clientApp) {
        if (string.IsNullOrWhiteSpace(clientApp.AppName)) throw new ArgumentNullException("Nazwa aplikacji nie może być pusta.");
        if (string.IsNullOrWhiteSpace(clientApp.AppId)) throw new ArgumentNullException("ID aplikacji nie może być puste.");

        if (_registeredApps.Exists(app => app.AppId == clientApp.AppId)) throw new Exception($"Aplikacja z ID '{clientApp.AppId}' jest już zarejestrowana.");
        if (_registeredApps.Exists(app => app.AppName == clientApp.AppName)) throw new Exception($"Aplikacja z nazwą '{clientApp.AppName}' jest już zarejestrowana.");
        _registeredApps.Add(clientApp);
    }
}