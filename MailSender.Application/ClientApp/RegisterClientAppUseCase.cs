using MailSender.Application.Abstractions.SharedConstants;
using MailSender.Application.Abstractions.ClientApp;
using MailSender.Domain.ClientApp;

namespace MailSender.Application.ClientApp;

public sealed class RegisterClientAppUseCase(
    IClientAppPasswordValidator passwordValidator,
    IClientAppAccessTokenIssuer accessTokenIssuer,
    IClientAppRegistry clientAppRegistry)
{
    public async Task<(RegisterClientAppResult?, int)> ExecuteAsync(
        RegisterClientAppCommand command,
        CancellationToken cancellationToken)
    {
        // Walidacja danych wejściowych
        if (string.IsNullOrWhiteSpace(command.AppId) ||
            string.IsNullOrWhiteSpace(command.AppName) ||
            string.IsNullOrWhiteSpace(command.Pass))
        {
            return (null, SharedConstants.LastTwoIndexNumbers);
        }

        // Walidacja hasła
        var (isValid, MyLastTwoIndexNumbers) = passwordValidator.ValidatePassword(command.Pass);

        if (!isValid)
        {
            // Niepoprawne hasło, zwracamy null i dwie ostatnie cyfry indeksu
            return (null, MyLastTwoIndexNumbers);
        }

        // sprawdzamy czy aplikacja o podanym ID lub nazwie istnieje w pamieci
        if (clientAppRegistry.GetRegisteredAppById(command.AppId) != null ||
            clientAppRegistry.GetRegisteredAppByName(command.AppName) != null)
        {
            // Aplikacja o podanym ID lub nazwie już istnieje, zwracamy null i dwie ostatnie cyfry indeksu
            return (null, SharedConstants.LastTwoIndexNumbers);
        }

        // Hasło jest poprawne, generujemy token dostępu dla aplikacji klienta
        var clientApplication = new ClientAppIdentity(command.AppId, command.AppName);
        var result = await accessTokenIssuer.IssueAsync(clientApplication, cancellationToken);

        // na koniec rejestracja aplikacji w pamieci.
        clientAppRegistry.RegisterClientApp(clientApplication);
        return (result, SharedConstants.LastTwoIndexNumbers);
    }
}
