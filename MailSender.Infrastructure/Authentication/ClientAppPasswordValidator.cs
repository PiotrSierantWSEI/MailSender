using MailSender.Application.Abstractions.SharedConstants;
using MailSender.Application.Abstractions.ClientApp;
using MailSender.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MailSender.Infrastructure.Authentication;

// PROCES WALIDACJI HASŁA APLIKACJI KLIENTA:
// 1. Pobieramy listę dozwolonych haseł z konfiguracji (ClientAppsOptions) i iterujemy przez nią, porównując każde hasło z hasłem przekazanym przez klienta. Tutaj mamy hasła z naszą końcówką indeksu.
// 2. Jeśli znajdziemy dopasowanie, zwracamy true oraz indeks (indeks nie bedzie tu potrzebny).
// 3. Jeśli nie znajdziemy dopasowania po sprawdzeniu wszystkich haseł, zwracamy false oraz SharedConstants.LastTwoIndexNumbers, która wskazuje moje ostatnie dwie cyfry indeksu.
public sealed class ClientAppPasswordValidator(IOptions<ClientAppsOptions> options) : IClientAppPasswordValidator
{
    private readonly ClientAppsOptions _options = options.Value;

    public (bool IsValid, int PasswordIndex) ValidatePassword(string password)
    {
        for (int i = 0; i < _options.Passwords.Length; i++)
        {
            if (_options.Passwords[i] == password)
            {
                return (true, i);
            }
        }

        return (false, SharedConstants.LastTwoIndexNumbers);
    }
}
