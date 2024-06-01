using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibrarySystemContext _context;

    public AuthorRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Author author)
    {
        _context.Authors.Add(author);
    }

    public async Task<Author?> Get(Guid id)
    {
        return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Author>> GetAll()
    {
        return await _context.Authors.ToListAsync();
    }
}
