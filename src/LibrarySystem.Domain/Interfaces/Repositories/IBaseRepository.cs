namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBaseRepository
{
    Task<int> SaveAsync();
}
