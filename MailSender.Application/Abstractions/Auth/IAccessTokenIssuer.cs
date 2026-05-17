using MailSender.Application.Auth;
using MailSender.Domain.Users;

namespace MailSender.Application.Abstractions.Auth;

// Interfejs definiujący wydawcę tokenów dostępu dla użytkowników
public interface IAccessTokenIssuer
{
    Task<AuthenticateUserResult> IssueAsync(UserIdentity user, CancellationToken cancellationToken);
}
