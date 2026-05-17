using MailSender.Domain.Users;

namespace MailSender.Application.Abstractions.Auth;

// Interfejs definiujący walidator poświadczeń użytkownika
public interface ICredentialValidator
{
    Task<UserIdentity?> ValidateAsync(string username, string password, CancellationToken cancellationToken);
}
