using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LibrarySystem.Domain.Exceptions.HTTP;

namespace LibrarySystem.Application.Core.Utilities;

public static class JwtUtils
{
    public static string Parse(string? header)
    {
        if (header is null)
        {
            throw new BadRequest400Exception("Authorization header is missing. Expected Format: BEARER <TOKEN>");
        }

        const string BEARER_PREFIX = "Bearer ";
        if (!header.StartsWith(BEARER_PREFIX, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequest400Exception("Invalid authorization header format. Expected format: BEARER <TOKEN>");
        }

        string token = header[BEARER_PREFIX.Length..].Trim();

        if (string.IsNullOrEmpty(token))
        {
            throw new BadRequest400Exception("Token is missing in the authorization header.");
        }

        return token;
    }

    public static IEnumerable<Claim> ParsePayload(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var payload = handler.ReadJwtToken(token).Claims;

        return payload;
    }

    public static string? ParseFromPayload(string token, string key)
    {
        var payload = ParsePayload(token);

        return payload.FirstOrDefault(c => c.Type.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
