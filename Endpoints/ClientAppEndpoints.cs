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

        // Zwracamy błąd 403 Forbidden, jeśli hasło jest nieprawidłowe (MyLastTwoIndexNumbers jest równy SharedConstants.LastTwoIndexNumbers)
        if (result is null)
        {
            // todo: nie obslugujemy rozroznienia miedzy niepoprawnym haslem a istniejaca aplikacja o takim samym id lub nazwie. Nic nie ma na ten temat w zadaniu.
            return Results.Json(
                new { error = $"Invalid index-based password {MyLastTwoIndexNumbers}" },
                statusCode: StatusCodes.Status403Forbidden);
        }

        var response = new RegisterClientAppResponse(request.AppId, request.AppName, result.Key);
        // Results.Created($"/client-app/{request.AppId}", response); -> mozna w przyszlosci zmienic na to gdy bedziemy mieli endpoint do pobierania danych aplikacji klienta po id.
        return Results.Ok(response);
    }
}
