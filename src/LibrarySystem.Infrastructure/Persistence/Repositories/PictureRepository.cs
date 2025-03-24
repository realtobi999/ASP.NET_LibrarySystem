using System.Linq.Expressions;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class PictureRepository : IPictureRepository
{
    private readonly LibrarySystemContext _context;

    public PictureRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Picture picture)
    {
        _context.Pictures.Add(picture);
    }

    public void Delete(Picture picture)
    {
        _context.Pictures.Remove(picture);
    }

    public void DeleteWhere(Expression<Func<Picture, bool>> expression)
    {
        _context.Pictures.RemoveRange(_context.Pictures.Where(expression));
    }
}