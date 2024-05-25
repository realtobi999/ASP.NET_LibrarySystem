using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

    public IEnumerable<Claim> ParseTokenPayload(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var payload = handler.ReadJwtToken(token).Claims;

        return payload;
    }
}
