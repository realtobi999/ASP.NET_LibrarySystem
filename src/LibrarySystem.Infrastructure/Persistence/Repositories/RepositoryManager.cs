using LibrarySystem.Domain.Exceptions.Common;
using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IRepositoryFactory _factory;
    private readonly LibrarySystemContext _context;

    public RepositoryManager(IRepositoryFactory factory, LibrarySystemContext context)
    {
        _factory = factory;
        _context = context;
    }

    public IUserRepository User => _factory.CreateUserRepository();

    public IEmployeeRepository Employee => _factory.CreateEmployeeRepository();

    public IAuthorRepository Author => _factory.CreateAuthorRepository();

    public IGenreRepository Genre => _factory.CreateGenreRepository();

    public IBookRepository Book => _factory.CreateBookRepository();

    public IBorrowRepository Borrow => _factory.CreateBorrowRepository();

    public IBookReviewRepository BookReview => _factory.CreateBookReviewRepository();

    public IWishlistRepository Wishlist => _factory.CreateWishlistRepository();

    public IPictureRepository Picture => _factory.CreatePictureRepository();

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task SaveSafelyAsync()
    {
        var affected = await this.SaveAsync();

        if (affected == 0)
            throw new ZeroRowsAffectedException();
    }
}