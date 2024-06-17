using LibrarySystem.Application.Services;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Tests.Integration.Extensions;

public static class JwtTestExtensions
{
    public static Jwt Create()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .Build();

        var jwtIssuer = configuration.GetSection("Jwt:Issuer").Value;
        var jwtKey = configuration.GetSection("Jwt:Key").Value;

        if (string.IsNullOrEmpty(jwtIssuer))
        {
            throw new ArgumentNullException(nameof(jwtIssuer), "JWT Issuer configuration is missing");
        }
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new ArgumentNullException(nameof(jwtKey), "JWT Key configuration is missing");
        }

        return new Jwt(jwtIssuer, jwtKey);
    }
}
