namespace TaskManager.Api.Endpoints;

public static class EndpointExtensions
{
    public static WebApplication MapAllEndpoints(this WebApplication app)
    {
        app.MapTaskEndpoints();
        return app;
    }
}