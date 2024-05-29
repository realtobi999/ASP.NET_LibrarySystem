using LibrarySystem.Application;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Exceptions;

namespace LibrarySystem.Presentation;

public class EmployeeAuthenticationMiddleware
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

        var header = context.Request.Headers.Authorization.FirstOrDefault() ?? throw new BadRequestException("Missing header: Bearer <JWT_TOKEN>");
        var splitHeader = header.Split(" ");
        
        if (splitHeader.Length != 2)
        {
            throw new BadRequestException("Bad authorization header format, try: Bearer <JWT_TOKEN>");
        }
        if (splitHeader.ElementAt(0).ToUpper() != "BEARER")
        {
            throw new BadRequestException("Bad authorization header format, try: Bearer <JWT_TOKEN>");
        }

        var token = splitHeader.ElementAt(1);
        var payload = JwtToken.ParsePayload(token);

        // get the id from the jwt token
        var tokenEmployeeId = payload.FirstOrDefault(c => c.Type.ToUpper() == "EMPLOYEEID")?.Value;

        // get the id from the route middleware
        var routeEmployeeId = context.Request.RouteValues.FirstOrDefault(v => v.Key.ToUpper() == "EMPLOYEEID").Value as string;

        if (tokenEmployeeId != routeEmployeeId)
        {
            throw new NotAuthorizedException("Not Authorized!");
        }

        await _next(context);
    }
}
