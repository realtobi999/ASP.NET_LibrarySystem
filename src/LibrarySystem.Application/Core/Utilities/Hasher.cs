using System.Security.Cryptography;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Application.Core.Utilities;

public class Hasher : IHasher
{
    private const int SaltSize = 128 / 8;  // size of the salt in bytes
    private const int HashSize = 256 / 8;  // size of the hash in bytes
    private const int Iterations = 10_000; // number of iterations for PBKDF2
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;
    private static readonly char Delimiter = ';';

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithm, HashSize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool Compare(string plainPassword, string hashedPassword)
    {
        var parts = hashedPassword.Split(Delimiter);
        if (parts.Length != 2)
        {
            throw new FormatException("Invalid hash format.");
        }

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        var hashOfInput = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, Iterations, HashAlgorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, hashOfInput);
    }
}