namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IStaffRepository Staff { get; }
    Task<int> SaveAsync(); 
}
