using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LibrarySystem.Presentation.Middlewares.Filters;

public class SwaggerAuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType is null)
        {
            return;
        }

        // check if the endpoint has an authorize attribute
        var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                                    || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        if (!hasAuthorizeAttribute)
        {
            return;
        }

        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        var scheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        operation.Security =
        [
            new OpenApiSecurityRequirement
            {
                [scheme] = Array.Empty<string>()
            }
        ];
    }
}