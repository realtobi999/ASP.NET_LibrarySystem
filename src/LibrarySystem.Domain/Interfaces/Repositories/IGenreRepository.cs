using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IGenreRepository : IBaseRepository<Genre>
{
    Task<Genre?> GetAsync(Guid id);
    Genre? Get(Guid id);
}
