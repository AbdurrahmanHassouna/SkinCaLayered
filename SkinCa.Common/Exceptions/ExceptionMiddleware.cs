using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SkinCa.Common.Exceptions;

public class ExceptionMiddleware:IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private static (int statusCode, string userMessage) HandleUnexpectedError(HttpContext context, Exception exception,ILogger<ExceptionMiddleware> _logger)
    {
        _logger.LogCritical(exception, 
            "Unexpected error | UserId: {UserId} | Path: {Path}",
            context.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            context.Request.Path);
        return (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
    }
    private static (int statusCode, string userMessage) HandleNotFoundError(HttpContext context, Exception exception,ILogger<ExceptionMiddleware> _logger)
    {
        _logger.LogWarning(exception, 
            "NotFound error | UserId: {UserId} | Path: {Path}",
            context.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            context.Request.Path);
        return (StatusCodes.Status404NotFound, exception.Message);
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError( exception,"userId: {UserId} | Path: {Path}",
            context.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            context.Request.Path);
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, userMessage) = exception switch
        {
            NotFoundException => HandleNotFoundError(context, exception, _logger),
            RepositoryException repoEx => (StatusCodes.Status500InternalServerError, repoEx.Message),
            ServiceException serviceEx => (StatusCodes.Status500InternalServerError, serviceEx.Message),
            _ =>HandleUnexpectedError(context, exception, _logger)
        };

        response.StatusCode = statusCode;

        var errorResponse = new
        {
            response.StatusCode,
            Message = userMessage 
        };

        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
