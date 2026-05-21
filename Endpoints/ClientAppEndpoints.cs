using MailSender.Application.ClientApp;
using MailSender.Contracts.ClientApp;

namespace MailSender.Endpoints;

public static class ClientAppEndpoints
{
    public static IEndpointRouteBuilder MapClientAppEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/client-app/register",
                (RegisterClientAppRequest request, RegisterClientAppUseCase useCase, CancellationToken cancellationToken) =>
                    HandleRegisterClientApp(request, useCase, cancellationToken))
            .AllowAnonymous() // Endpoint publiczny, nie wymaga tokenów
            .WithName("RegisterClientApp")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> HandleRegisterClientApp(
        RegisterClientAppRequest request,
        RegisterClientAppUseCase useCase,
        CancellationToken cancellationToken)
    {
        var (result, MyLastTwoIndexNumbers) = await useCase.ExecuteAsync(
            new RegisterClientAppCommand(request.AppId, request.AppName, request.Pass),
            cancellationToken);

        if (result is null || result.Error != RegisterClientAppError.None)
        {
            return Results.Json(
                new { error = $"Error: {result?.Error.ToString() ?? "Unknown error"}, Index-based password info: {MyLastTwoIndexNumbers}" },
                statusCode: StatusCodes.Status403Forbidden);
        }

        if (result.Error == RegisterClientAppError.InvalidInput)
        {
            return Results.Json(
                new { error = $"Invalid input data for client app registration" },
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (result.Error == RegisterClientAppError.InvalidPassword)
        {
            return Results.Json(
                new { error = $"Invalid index-based password {MyLastTwoIndexNumbers}" },
                statusCode: StatusCodes.Status403Forbidden);
        }

        if (result.Error == RegisterClientAppError.AppAlreadyExists)
        {
            return Results.Json(
            new { error = $"client app duplication. Existing {request.AppId} {request.AppName}" },
                statusCode: StatusCodes.Status409Conflict);
        }

        if (result.Key is null || result.ExpiresAtUtc is null)
        {
            return Results.Json(
                new { error = $"Failed to generate access token for client app" },
                statusCode: StatusCodes.Status500InternalServerError);
        }

        // Zwracamy błąd 403 Forbidden, jeśli hasło jest nieprawidłowe (MyLastTwoIndexNumbers jest równy SharedConstants.LastTwoIndexNumbers)
        // if (result is null || result.Error )
        // {
        //     return Results.Json(
        //         new { error = $"Invalid index-based password {MyLastTwoIndexNumbers}" },
        //         statusCode: StatusCodes.Status403Forbidden);
        // }

        var response = new RegisterClientAppResponse(request.AppId, request.AppName, result.Key);
        // Results.Created($"/client-app/{request.AppId}", response); -> mozna w przyszlosci zmienic na to gdy bedziemy mieli endpoint do pobierania danych aplikacji klienta po id.
        return Results.Ok(response);
    }
}
