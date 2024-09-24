using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IRepositoryFactory _factory;

    public RepositoryManager(IRepositoryFactory factory)
    {
        _factory = factory;
    }

    public IUserRepository User => _factory.CreateUserRepository();

    public IEmployeeRepository Employee => _factory.CreateEmployeeRepository();

    public IAuthorRepository Author => _factory.CreateAuthorRepository();

    public IGenreRepository Genre => _factory.CreateGenreRepository();

    public IAssociationsRepository Associations => _factory.CreateAssociationsRepository();

    public IBookRepository Book => _factory.CreateBookRepository();

    public IBorrowRepository Borrow => _factory.CreateBorrowRepository();

    public IBookReviewRepository BookReview => _factory.CreateBookReviewRepository();

    public IWishlistRepository Wishlist => _factory.CreateWishlistRepository();

    public IPictureRepository Picture => _factory.CreatePictureRepository();

    private IBaseRepository _base => _factory.CreateBaseRepository();

    public Task<int> SaveAsync()
    {
        return _base.SaveAsync();
    }
}
