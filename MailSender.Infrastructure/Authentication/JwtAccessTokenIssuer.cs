using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MailSender.Application.Abstractions.Auth;
using MailSender.Application.Auth;
using MailSender.Domain.Users;
using MailSender.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MailSender.Infrastructure.Authentication;

// PROCES TWORZENIA TOKENA JWT:
// 1. Pobieramy konfigurację JWT z pliku appsettings.json i mapujemy ją do obiektu JwtOptions.
// 2. Tworzymy listę claims na podstawie informacji o użytkowniku.
// 3. Tworzymy klucz symetryczny na podstawie skonfigurowanego klucza (JwtOptions.Key) i definiujemy algorytm podpisu (HmacSha256). 
// 4. Tworzymy token JWT (JwtSecurityToken) z określonymi parametrami: issuer, audience, claims, czasem wygaśnięcia i podpisem.
public sealed class JwtAccessTokenIssuer(IOptions<JwtOptions> options) : IAccessTokenIssuer
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public Task<AuthenticateUserResult> IssueAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        // Obliczamy czas wygaśnięcia tokena na podstawie aktualnego czasu i wartości ExpiresInMinutes z konfiguracji.
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes);
        // Dane z claimsów
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Name, user.Username),
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

        return Task.FromResult(new AuthenticateUserResult(accessToken, expiresAt));
    }
}
