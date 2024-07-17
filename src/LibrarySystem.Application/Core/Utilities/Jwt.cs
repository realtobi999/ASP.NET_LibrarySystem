using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibrarySystem.Domain.Exceptions.BadRequest;
using LibrarySystem.Domain.Interfaces.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Core.Utilities;

public class Jwt : IJwt
{
    public string Issuer { get; set; }
    public string Key { get; set; }

    public Jwt(string issuer, string key)
    {
        Issuer = issuer;
        Key = key;
    }

    public string Generate(IEnumerable<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = Issuer,
            Audience = Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}