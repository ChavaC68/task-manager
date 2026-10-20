using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api.Middleware;

/// <summary>
/// מיירט חריגות שלא טופלו ומחזיר תגובת ProblemDetails אחידה (RFC 7807).
/// לא חושף פרטים פנימיים ללקוח.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex)
        {
            // גוף בקשה שאינו תקין (למשל JSON שבור) — שגיאת לקוח, לא תקלת שרת.
            _logger.LogWarning(ex, "Malformed request on {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteProblemAsync(
                context,
                StatusCodes.Status400BadRequest,
                "הבקשה אינה תקינה.",
                "גוף הבקשה אינו בפורמט חוקי.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteProblemAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "אירעה שגיאה בלתי צפויה בשרת.",
                "נא לנסות שוב מאוחר יותר.");
        }
    }

    /// <summary>כותב תגובת ProblemDetails אחידה, אם התגובה טרם החלה להישלח.</summary>
    private static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {
        // אם התגובה כבר החלה — אין אפשרות לשנות אותה.
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        });
    }
}