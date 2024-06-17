using LibrarySystem.Application.Services;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Application.Factories;

public class JwtFactory
{
    public static Jwt CreateInstance(IConfiguration configuration)
    {
        var issuer = configuration.GetSection("Jwt:Issuer").Value;
        var key = configuration.GetSection("Jwt:Key").Value;

        if (string.IsNullOrEmpty(issuer))
        {
            throw new ArgumentNullException(nameof(issuer), "JWT Issuer configuration is missing");
        }
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "JWT Key configuration is missing");
        }

        return new Jwt(issuer, key);
    }
}
