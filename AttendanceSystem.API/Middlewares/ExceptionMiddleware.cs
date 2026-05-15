using AttendanceSystem.Application;
using System.Net;
using System.Text.Json;

namespace AttendanceSystem.API;

public class ExceptionMiddleware
{
    readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

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

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
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

        context.Response.StatusCode = (int)statusCode;

        ApiResponse<string> response =
            ApiResponse<string>.FailureResponse(
                exception.Message);

        string json =
            JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}