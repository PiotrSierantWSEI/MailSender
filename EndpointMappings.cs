using MailSender.Endpoints;

namespace MailSender;

public static class EndpointMappings
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        // Mapowanie endpointów - dodajemy odpowiednio nowe endpointy.
        app.MapAuthEndpoints();
        app.MapClientAppEndpoints();
        app.MapMailSendEndpoints();
        app.GetClientAppLogsEndpoints();
        app.GetClientAppLogByIdEndpoints();

        return app;
    }
}
