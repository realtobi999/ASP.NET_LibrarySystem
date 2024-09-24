using System.Text.Json.Nodes;
using LibrarySystem.Domain.Exceptions.HTTP;

namespace LibrarySystem.Presentation.Middlewares;

public abstract class AuthenticationMiddlewareBase
{
    protected async Task<string?> ExtractKeyFromRouteOrBodyAsync(HttpContext context, string key)
    {
        var requestUserId = context.Request.RouteValues.FirstOrDefault(v => v.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                                                       .Value?.ToString();
        if (requestUserId is null)
        {
            using (var reader = new StreamReader(context.Request.Body))
            {
                var requestBody = await reader.ReadToEndAsync();

                var jsonBody = JsonNode.Parse(requestBody) as JsonObject
                               ?? throw new BadRequest400Exception($"Please provide a body with a {key} field.");

                requestUserId = jsonBody.FirstOrDefault(p => p.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                                        .Value?.ToString();

                if (requestUserId is null)
                {
                    throw new BadRequest400Exception($"{key} is missing in both the route and the body.");
                }

                // Reset the request body stream position for further processing
                context.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));
            }
        }

        return requestUserId;
    }
}
