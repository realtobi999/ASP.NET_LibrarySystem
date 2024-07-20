using System.Drawing;
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
        _context.Add(picture);
    }

    public void Delete(Picture picture)
    {
        _context.Remove(picture);
    }
}
