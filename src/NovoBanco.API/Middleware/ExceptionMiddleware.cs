using System.Net;
using System.Text.Json;

namespace NovoBanco.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Internal server error";

        switch (exception.Message)
        {
            case var msg when msg.Contains("not found", StringComparison.OrdinalIgnoreCase):
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case var msg when msg.Contains("duplicate", StringComparison.OrdinalIgnoreCase):
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                break;

            case var msg when msg.Contains("insufficient", StringComparison.OrdinalIgnoreCase):
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case var msg when msg.Contains("invalid", StringComparison.OrdinalIgnoreCase):
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case var msg when msg.Contains("required", StringComparison.OrdinalIgnoreCase):
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case var msg when msg.Contains("greater than zero", StringComparison.OrdinalIgnoreCase):
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
        }

        var response = new
        {
            error = message,
            status = (int)statusCode
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
