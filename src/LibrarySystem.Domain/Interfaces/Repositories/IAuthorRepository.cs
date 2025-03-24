using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IAuthorRepository : IBaseRepository<Author>
{
    Task<Author?> GetAsync(Guid id);
    Author? Get(Guid id);
}