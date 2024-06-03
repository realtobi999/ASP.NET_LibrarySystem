using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public void Delete(Genre genre)
    {
        _context.Genres.Remove(genre);
    }

    public async Task<Genre?> Get(Guid id)
    {
        return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await _context.Genres.ToListAsync();    
    }
}
