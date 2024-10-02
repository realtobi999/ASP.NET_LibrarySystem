namespace LibrarySystem.Domain.Interfaces.Common;

/// <summary>
/// Provides functionality for hashing passwords and comparing plain passwords with hashed passwords.
/// </summary>
public interface IHasher
{
    /// <summary>
    /// Hashes the specified plain text.
    /// </summary>
    /// <param name="text">The plain text to be hashed.</param>
    /// <returns>A hashed representation of the text.</returns>
    string Hash(string text);

    /// <summary>
    /// Compares a plain text with a hashed text to determine if they match.
    /// </summary>
    /// <param name="plain">The plain text to compare.</param>
    /// <param name="hashed">The hashed text to compare against.</param>
    /// <returns><c>true</c> if the plain text matches the hashed text; otherwise, <c>false</c>.</returns>
    bool Compare(string plain, string hashed);
}
