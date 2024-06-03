using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly LibrarySystemContext _context;

    public GenreRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Genre genre)
    {
        _context.Genres.Add(genre);        
    }
}
