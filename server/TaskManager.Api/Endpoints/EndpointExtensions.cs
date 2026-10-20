namespace TaskManager.Api.Endpoints;

/// <summary>רישום מרוכז של כל קבוצות ה-Endpoints.</summary>
public static class EndpointExtensions
{
    public static WebApplication MapAllEndpoints(this WebApplication app)
    {
        app.MapTaskEndpoints();
        return app;
    }
}