using MailSender.Application.ClientApp;
using MailSender.Domain.ClientApp;

namespace MailSender.Application.Abstractions.ClientApp;

// Interfejs definiujący dostęp dla aplikacji klienta
public interface IClientAppAccessTokenIssuer
{
    Task<RegisterClientAppResult> IssueAsync(ClientAppIdentity clientApp, CancellationToken cancellationToken);
}
