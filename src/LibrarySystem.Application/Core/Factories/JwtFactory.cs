using LibrarySystem.Application.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Application.Core.Factories;

public static class JwtFactory
{
    public static Jwt CreateInstance(IConfiguration configuration)
    {
        var issuer = configuration.GetSection("Jwt:Issuer").Value;
        var key = configuration.GetSection("Jwt:Key").Value;

        if (string.IsNullOrEmpty(issuer))
        {
            throw new NullReferenceException("JWT Issuer configuration is missing");
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new NullReferenceException("JWT Key configuration is missing");
        }

        return new Jwt(issuer, key);
    }
}