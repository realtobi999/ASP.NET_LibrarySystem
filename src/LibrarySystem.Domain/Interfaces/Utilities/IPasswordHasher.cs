namespace LibrarySystem.Domain.Interfaces.Utilities;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Compare(string plainPassword, string hashedPassword);
}
