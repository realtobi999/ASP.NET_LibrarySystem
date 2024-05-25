namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    Task<int> SaveAsync(); 
}
