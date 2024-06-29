using System.Text.Json.Nodes;
using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Exceptions;

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
        var token = Jwt.Parse(context.Request.Headers.Authorization);
        var jwtUserId = Jwt.ParseFromPayload(token, "UserId");

        // try to parse the token from the request - first from route parameter, then from json body
        var requestUserId = await ExtractKeyFromRouteOrBodyAsync(context, "UserId");

        // validate that the id from the token matches the id from the route or body
        if (jwtUserId != requestUserId)
        {
            throw new NotAuthorizedException("Not Authorized!");
        }

        await _next(context);
    }
}