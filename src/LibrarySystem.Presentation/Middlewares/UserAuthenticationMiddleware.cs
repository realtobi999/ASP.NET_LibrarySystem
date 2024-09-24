using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Exceptions.HTTP;

namespace LibrarySystem.Presentation.Middlewares;

public class UserAuthenticationMiddleware : AuthenticationMiddlewareBase
{
    private readonly RequestDelegate _next;

    public UserAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // skip if the endpoint doesn't have the specific middleware attribute
        if (context.GetEndpoint()?.Metadata.GetMetadata<UserAuthAttribute>() is null)
        {
            await _next(context);
            return;
        }

        // parse the jwt token from the request header and get the token
        var token = JwtUtils.Parse(context.Request.Headers.Authorization);
        var jwtUserId = JwtUtils.ParseFromPayload(token, "UserId");

        // try to parse the token from the request - first from route parameter, then from json body
        var requestUserId = await ExtractKeyFromRouteOrBodyAsync(context, "UserId");

        // validate that the id from the token matches the id from the route or body
        if (jwtUserId != requestUserId)
        {
            throw new NotAuthorized401Exception();
        }

        await _next(context);
    }
}