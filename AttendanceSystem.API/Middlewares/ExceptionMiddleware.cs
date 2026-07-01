using AttendanceSystem.Application;
using System.Net;
using System.Text.Json;

namespace AttendanceSystem.API;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    readonly RequestDelegate _next = next;
    readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            ForbiddenException => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError
        };

        // Known/expected exceptions -> Warning. Everything else -> Error (with full stack trace).
        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception on {Path}", context.Request.Path);
        else
            _logger.LogWarning("{ExceptionType} on {Path}: {Message}",
                exception.GetType().Name, context.Request.Path, exception.Message);

        context.Response.StatusCode = (int)statusCode;

        ApiResponse<string> response = ApiResponse<string>.FailureResponse(exception.Message);
        string json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}