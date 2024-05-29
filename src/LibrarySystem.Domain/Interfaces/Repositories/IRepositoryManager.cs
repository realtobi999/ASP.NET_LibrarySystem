namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IEmployeeRepository Employee { get; }
    Task<int> SaveAsync(); 
}
