namespace LibrarySystem.Domain.Interfaces.Common;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Compare(string plainPassword, string hashedPassword);
}
