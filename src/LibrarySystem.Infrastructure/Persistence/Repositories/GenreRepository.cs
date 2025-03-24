using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class GenreRepository : BaseRepository<Genre>, IGenreRepository
{
    public GenreRepository(LibrarySystemContext context) : base(context)
    {
    }

    public Genre? Get(Guid id)
    {
        return Context.Genres.FirstOrDefault(g => g.Id == id);
    }

    public async Task<Genre?> GetAsync(Guid id)
    {
        return await this.GetAsync(g => g.Id == id);
    }
}