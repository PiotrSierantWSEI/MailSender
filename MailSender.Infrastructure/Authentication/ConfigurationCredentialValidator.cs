using MailSender.Application.Abstractions.Auth;
using MailSender.Domain.Users;
using MailSender.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MailSender.Infrastructure.Authentication;

// PROCES WALIDACJI CREDENTIALI UŻYTKOWNIKA:
// 1. Pobieramy dane użytkownika z konfiguracji (DemoUserOptions) i porównujemy je z danymi przekazanymi przez klienta (username i password).
// 2. Jeśli dane się nie zgadzają, zwracamy null, co oznacza nieudane uwierzytelnienie.
// 3. Jeśli dane są poprawne, tworzymy i zwracamy obiekt UserIdentity.
public sealed class ConfigurationCredentialValidator(IOptions<DemoUserOptions> options) : ICredentialValidator
{
    private readonly DemoUserOptions _demoUser = options.Value;

    public Task<UserIdentity?> ValidateAsync(string username, string password, CancellationToken cancellationToken)
    {
        if (!string.Equals(username, _demoUser.Username, StringComparison.Ordinal) ||
            !string.Equals(password, _demoUser.Password, StringComparison.Ordinal))
        {
            Console.WriteLine("[ConfigurationCredentialValidator] Credential validation FAILED");
            return Task.FromResult<UserIdentity?>(null);
        }

        Console.WriteLine("[ConfigurationCredentialValidator] Credential validation SUCCEEDED");
        return Task.FromResult<UserIdentity?>(new UserIdentity(_demoUser.Username));
    }
}
