using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

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
}
