using MailSender.Application.Auth;
using MailSender.Contracts.Auth;

namespace MailSender.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/auth/token",
                (IssueTokenRequest request, AuthenticateUserUseCase useCase, CancellationToken cancellationToken) =>
                    HandleIssueToken(request, useCase, cancellationToken))
            .AllowAnonymous() // publiczny endpoint
            .WithName("IssueToken")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> HandleIssueToken(
        IssueTokenRequest request,
        AuthenticateUserUseCase useCase,
        CancellationToken cancellationToken)
    {
        // przekazujemy dane z requestu do use case'a
        var result = await useCase.ExecuteAsync(
            new AuthenticateUserCommand(request.Username, request.Password),
            cancellationToken);

        // jesli wynik jest null, to znaczy ze uwierzytelnianie sie nie powiodlo, zwracamy 401 Unauthorized
        // w przeciwnym wypadku zwracamy 200 OK z tokenem i czasem wygaśnięcia tokena (mozna pomyslec nad Created bo tworzymy zasob tokena, ale to juz kwestia dyskusyjna)
        return result is null
            ? Results.Unauthorized()
            : Results.Ok(new IssueTokenResponse(result.AccessToken, result.ExpiresAtUtc));
    }
}
