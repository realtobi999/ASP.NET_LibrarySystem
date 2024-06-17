using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Services;

public class JwtToken : IJwtToken
{
    private readonly string _jwtIssuer;
    private readonly string _jwtKey;

    public JwtToken(string jwtIssuer, string jwtKey)
    {
        _jwtIssuer = jwtIssuer;
        _jwtKey = jwtKey;
    }

    public static string Parse(string? header)
    {
        if (header is null)
        {
            throw new BadRequestException("Authorization header is missing. Expected Format: BEARER <TOKEN>");
        }

        const string bearerPrefix = "Bearer ";
        if (!header.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Invalid authorization header format. Expected format: BEARER <TOKEN>");
        }

        string token = header.Substring(bearerPrefix.Length).Trim();

        if (string.IsNullOrEmpty(token))
        {
            throw new BadRequestException("Token is missing in the authorization header.");
        }

        return token;
    }

    public string Generate(IEnumerable<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(_jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _jwtIssuer,
            Audience = _jwtIssuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public static IEnumerable<Claim> ParsePayload(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var payload = handler.ReadJwtToken(token).Claims;

        return payload;
    }
}
