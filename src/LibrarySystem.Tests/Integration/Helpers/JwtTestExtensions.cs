using LibrarySystem.Application.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class JwtTestExtensions
{
    public static Jwt Create()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json")
                                                      .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                                                      .Build();

        var jwtIssuer = configuration.GetSection("Jwt:Issuer").Value;
        var jwtKey = configuration.GetSection("Jwt:Key").Value;

        if (string.IsNullOrEmpty(jwtIssuer))
        {
            throw new NullReferenceException("JWT Issuer configuration is missing");
        }
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new NullReferenceException("JWT Key configuration is missing");
        }

        return new Jwt(jwtIssuer, jwtKey);
    }
}
