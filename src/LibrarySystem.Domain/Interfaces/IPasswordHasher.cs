namespace LibrarySystem.Domain.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Compare(string plainPassword, string hashedPassword);
}
