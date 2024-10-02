using System.Security.Claims;

namespace LibrarySystem.Domain.Interfaces.Common;

/// <summary>
/// Provides functionality for generating JSON Web Tokens (JWTs).
/// </summary>
public interface IJwt
{
    /// <summary>
    /// Gets the issuer of the JWT.
    /// </summary>
    string Issuer { get; }

    /// <summary>
    /// Gets the secret key used to sign the JWT.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Generates a JSON Web Token (JWT) based on the provided claims.
    /// </summary>
    /// <param name="claims">A collection of claims to be included in the JWT.</param>
    /// <returns>A signed JWT as a string.</returns>
    string Generate(IEnumerable<Claim> claims);
}
