using System.Net;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace LibrarySystem.Presentation.Middlewares.Handlers;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken token)
    {
        if (exception is IHttpException httpException)
        {
            await HandleHttpException(context, httpException, token);
        }
        else
        {
            await HandleGeneralException(context, exception, token);
        }

        _logger.LogError("An error occurred at {Timestamp} while processing the request: {Method} {Path} - {Message}",
            DateTime.UtcNow,
            context.Request.Method,
            context.Request.Path,
            exception.Message);


        return false;
    }

    private static async Task HandleHttpException(HttpContext context, IHttpException exception, CancellationToken token)
    {
        var error = new ErrorMessage
        {
            StatusCode = exception.StatusCode,
            Type = exception.GetType().Name,
            Title = exception.Title,
            Detail = exception.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        await WriteErrorAsync(context, error, token);
    }

    private static async Task HandleGeneralException(HttpContext context, Exception exception, CancellationToken token)
    {
        var error = new ErrorMessage
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Type = exception.GetType().Name,
            Title = "An unexpected internal error occurred",
            Detail = exception.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        await WriteErrorAsync(context, error, token);
    }

    private static async Task WriteErrorAsync(HttpContext context, ErrorMessage error, CancellationToken token)
    {
        context.Response.StatusCode = error.StatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(error, token);
    }
}
