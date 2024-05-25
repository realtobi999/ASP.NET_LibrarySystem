using System.Security.Claims;

namespace LibrarySystem.Domain.Interfaces;

public interface IJwtToken
{
    string Generate(IEnumerable<Claim> claims);
}
