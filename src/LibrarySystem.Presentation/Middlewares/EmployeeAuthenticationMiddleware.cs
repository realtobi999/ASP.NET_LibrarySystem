﻿using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Exceptions.HTTP;

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
        var token = JwtUtils.Parse(context.Request.Headers.Authorization);
        var tokenEmployeeId = JwtUtils.ParseFromPayload(token, "EmployeeId");

        // try to parse the token from the request - first from route parameter, then from json body
        var requestEmployeeId = await ExtractKeyFromRouteOrBodyAsync(context, "EmployeeId");

        // validate that the id from the token matches the id from the route or body
        if (tokenEmployeeId != requestEmployeeId)
        {
            throw new NotAuthorized401Exception();
        }

        await _next(context);
    }
}