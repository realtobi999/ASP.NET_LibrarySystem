using System.Net;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace LibrarySystem.Presentation.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        InternalServerErrorException => StatusCodes.Status500InternalServerError,
                        _ => StatusCodes.Status500InternalServerError,
                    };

                    await context.Response.WriteAsync(new ErrorMessage()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                    }.ToString());
                }
            });
        });
    }
}
