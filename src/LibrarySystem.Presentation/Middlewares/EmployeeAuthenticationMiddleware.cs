using LibrarySystem.Application;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;

namespace LibrarySystem.Presentation.Middlewares;

public class EmployeeAuthenticationMiddleware : AuthenticationMiddlewareBase
{
    private readonly RequestDelegate _next;

    public EmployeeAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // skip if the endpoint doesnt have the specific middleware attribute
        if (context.GetEndpoint()?.Metadata.GetMetadata<EmployeeAuthAttribute>() is null)
        {
            await _next(context);
            return;
        }

        // parse the jwt token from the request header and get the token
        var token = Jwt.Parse(context.Request.Headers.Authorization);
        var tokenEmployeeId = Jwt.ParseFromPayload(token, "EmployeeId");

        // try to parse the token from the request - first from route parameter, then from json body
        var requestEmployeeId = await ExtractKeyFromRouteOrBodyAsync(context, "EmployeeId");

        // validate that the id from the token matches the id from the route or body
        if (tokenEmployeeId != requestEmployeeId)
        {
            throw new NotAuthorizedException("Not Authorized!");
        }

        await _next(context);
    }
}
