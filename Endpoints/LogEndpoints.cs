using MailSender.Application.Log;
// using MailSender.Contracts.Log; Response dto
using System.Security.Claims;


namespace MailSender.Endpoints;

public static class LogEndpoints
{
    public static IEndpointRouteBuilder GetClientAppLogsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/mail-log",
                (ClaimsPrincipal clientApp, LogUseCase useCase, CancellationToken cancellationToken) =>
                    HandleGetMailLogsClientAppById(clientApp, useCase, cancellationToken))
            .RequireAuthorization("ClientApp") // Wymagamy autroryzacji tokenem z register client app
            .WithName("GetMailLogs")
            .WithOpenApi();

        return app;
    }

    public static IEndpointRouteBuilder GetClientAppLogByIdEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/mail-log/{id}",
                (string id, ClaimsPrincipal clientApp, LogUseCase useCase, CancellationToken cancellationToken) =>
                    HandleGetMailLogClientAppById(id, clientApp, useCase, cancellationToken))
            .RequireAuthorization("ClientApp") // Wymagamy autroryzacji tokenem z register client app
            .WithName("GetMailLogsById")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> HandleGetMailLogsClientAppById(
        ClaimsPrincipal clientApp,
        LogUseCase useCase,
        CancellationToken cancellationToken)
    {
        var AppId = clientApp.FindFirst("app_id")?.Value;

        var result = await useCase.Execute(
            new LogCommand(AppId, LogId: null),
            cancellationToken);

        if (result.Error != null)
        {
            if (result.Error.Type == ErrorType.BadRequest)
            {
                return Results.BadRequest(result.Error);
            }
            else if (result.Error.Type == ErrorType.NotFound)
            {
                return Results.NotFound(result.Error);
            }
            else
            {
                return Results.Problem(result.Error.ErrorMessage);
            }
        }

        return Results.Ok(result.Logs);
    }

    private static async Task<IResult> HandleGetMailLogClientAppById(
        string id,
        ClaimsPrincipal clientApp,
        LogUseCase useCase,
        CancellationToken cancellationToken)
    {
        var AppId = clientApp.FindFirst("app_id")?.Value;

        var result = await useCase.Execute(
            new LogCommand(AppId, LogId: id),
            cancellationToken);

        if (result.Error != null)
        {
            if (result.Error.Type == ErrorType.BadRequest)
            {
                return Results.BadRequest(result.Error);
            }
            else if (result.Error.Type == ErrorType.NotFound)
            {
                return Results.NotFound(result.Error);
            }
            else
            {
                return Results.Problem(result.Error.ErrorMessage);
            }
        }

        return Results.Ok(result.Logs);
    }
}
