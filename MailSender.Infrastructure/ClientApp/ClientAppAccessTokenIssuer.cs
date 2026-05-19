using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailSender.Application.Abstractions.ClientApp;
using MailSender.Application.ClientApp;
using MailSender.Domain.ClientApp;
using MailSender.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MailSender.Infrastructure.ClientApp;

// PROCES TWORZENIA TOKENA JWT:
// 1. Pobieramy konfigurację JWT z pliku appsettings.json i mapujemy ją do obiektu JwtOptions.
// 2. Tworzymy listę claims na podstawie informacji o aplikacji klienckiej.
// 3. Tworzymy klucz symetryczny na podstawie skonfigurowanego klucza (JwtOptions.Key) i definiujemy algorytm podpisu (HmacSha256). 
// 4. Tworzymy token JWT (JwtSecurityToken) z określonymi parametrami: issuer, audience, claims, czasem wygaśnięcia i podpisem.
public sealed class ClientAppAccessTokenIssuer(IOptions<JwtOptions> options) : IClientAppAccessTokenIssuer
{
    private readonly JwtOptions _jwtOptions = options.Value;
    // Czas waznosci tokena to 90 dni wedlug zadania
    private const int ExpiresInDays = 90;

    public Task<RegisterClientAppResult> IssueAsync(ClientAppIdentity clientApp, CancellationToken cancellationToken)
    {
        // Obliczamy czas wygaśnięcia tokena na podstawie aktualnego czasu i ExpiresInDays.
        var expiresAt = DateTime.UtcNow.AddDays(ExpiresInDays);
        // Dane z claimsów
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, clientApp.AppId),
            new Claim("app_id", clientApp.AppId),
            new Claim("app_name", clientApp.AppName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Tworzymy klucz symetryczny na podstawie skonfigurowanego klucza (JwtOptions.Key) i definiujemy algorytm podpisu (HmacSha256).
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            SecurityAlgorithms.HmacSha256);

        // Tworzymy token JWT (JwtSecurityToken) z określonymi parametrami: issuer, audience, claims, czasem wygaśnięcia i podpisem.
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: signingCredentials);

        // Serializujemy token do formatu string, który będzie zwracany klientowi.
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(new RegisterClientAppResult(accessToken, expiresAt));
    }
}
