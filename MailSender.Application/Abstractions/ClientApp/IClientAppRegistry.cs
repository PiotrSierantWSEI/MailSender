using MailSender.Domain.ClientApp;

namespace MailSender.Application.Abstractions.ClientApp;

// Interfejs do trzymania informacji o zarejestrowanych aplikacjach
public interface IClientAppRegistry
{
    IReadOnlyList<ClientAppIdentity> GetAllRegisteredApps();
    ClientAppIdentity? GetRegisteredAppById(string appId);
    ClientAppIdentity? GetRegisteredAppByName(string appName);
    void RegisterClientApp(ClientAppIdentity clientApp);
}
