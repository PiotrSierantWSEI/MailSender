using MailSender.Application.MailSend;
using MailSender.Contracts.MailSend;
using System.Security.Claims;

namespace MailSender.Endpoints;

public static class MailSendEndpoints
{
    public static IEndpointRouteBuilder MapMailSendEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/mail/send",
                (ClaimsPrincipal clientApp, MailSendRequest request, MailSendAppUseCase useCase, CancellationToken cancellationToken) =>
                    HandleMailSend(clientApp, request, useCase, cancellationToken))
            .RequireAuthorization("ClientApp") // Wymagamy autroryzacji tokenem z register client app
            .WithName("SendMessage")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> HandleMailSend(
        ClaimsPrincipal clientApp,
        MailSendRequest request,
        MailSendAppUseCase useCase,
        CancellationToken cancellationToken)
    {
        // Odczyt danych z claimsów tokena JWT
        var appId = clientApp.FindFirst("app_id")?.Value;
        var appName = clientApp.FindFirst("app_name")?.Value;

        var result = await useCase.ExecuteAsync(
            new MailSendCommand(appId, appName, request.To, request.Subject, request.Body),
            cancellationToken);

        if (result.Status == MailSendStatus.Failure)
        {
            return Results.Problem(result.Error ?? "Unknown error", statusCode: 500);
        }
        
        return Results.Ok(result); // Kod 200 OK, mozna pomyslec o 202 Accepted jesli mail trafi do kolejki asynchronicznej
    }
}
