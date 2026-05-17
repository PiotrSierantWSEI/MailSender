using MailSender.Application.Abstractions.Auth;

namespace MailSender.Application.Auth;

public sealed class AuthenticateUserUseCase(
    ICredentialValidator credentialValidator,
    IAccessTokenIssuer accessTokenIssuer)
{
    public async Task<AuthenticateUserResult?> ExecuteAsync(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken)
    {
        // walidacja danych wejsciowych
        if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
        {
            return null;
        }

        // weryfikacja danych uzytkownika
        var user = await credentialValidator.ValidateAsync(command.Username, command.Password, cancellationToken);

        // jesli walidacja sie nie powiodla, zwracamy null, jesli sie powiodla to generujemy token JWT
        return user is null
            ? null
            : await accessTokenIssuer.IssueAsync(user, cancellationToken);
    }
}
