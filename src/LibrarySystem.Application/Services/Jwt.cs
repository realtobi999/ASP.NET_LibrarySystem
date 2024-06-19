using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Services;

public class Jwt : IJwt
{
    public string Issuer { get; set; }
    public string Key { get; set; }


    public Jwt(string issuer, string key)
    {
        Issuer = issuer;
        Key = key;
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
        var key = Encoding.ASCII.GetBytes(this.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = this.Issuer,
            Audience = this.Issuer,
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

    public static string? ParseFromPayload(string token, string key)
    {
        var payload = Jwt.ParsePayload(token);

        return payload.FirstOrDefault(c => c.Type.Equals(key, StringComparison.CurrentCultureIgnoreCase))?.Value;
    }
}
