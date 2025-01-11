using System.Security.Cryptography;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Application.Core.Utilities;

public class Hasher : IHasher
{
    private const int SALT_SIZE = 128 / 8;  // size of the salt in bytes
    private const int HASH_SIZE = 256 / 8;  // size of the hash in bytes
    private const int ITERATIONS = 10_000; // number of iterations for PBKDF2

    private readonly char _delimiter = ';';
    private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA3_512;

    public Hasher(HashAlgorithmName algorithm, char delimiter = ';')
    {
        _algorithm = algorithm;
        _delimiter = delimiter;
    }

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, _algorithm, HASH_SIZE);

        return string.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool Compare(string plainPassword, string hashedPassword)
    {
        var parts = hashedPassword.Split(_delimiter);
        if (parts.Length != 2)
        {
            throw new FormatException("Invalid hash format.");
        }

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        var hashOfInput = Rfc2898DeriveBytes.Pbkdf2(plainPassword, salt, ITERATIONS, _algorithm, HASH_SIZE);

        return CryptographicOperations.FixedTimeEquals(hash, hashOfInput);
    }
}