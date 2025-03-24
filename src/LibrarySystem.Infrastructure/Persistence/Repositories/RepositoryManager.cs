using LibrarySystem.Domain.Exceptions.Common;
using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;
    private readonly Lazy<IAuthorRepository> _authorRepository;
    private readonly Lazy<IGenreRepository> _genreRepository;
    private readonly Lazy<IBookRepository> _bookRepository;
    private readonly Lazy<IBorrowRepository> _borrowRepository;
    private readonly Lazy<IBookReviewRepository> _bookReviewRepository;
    private readonly Lazy<IWishlistRepository> _wishlistRepository;
    private readonly Lazy<IPictureRepository> _pictureRepository;
    private readonly LibrarySystemContext _context;

    public RepositoryManager(IRepositoryFactory factory, LibrarySystemContext context)
    {
        _userRepository = new Lazy<IUserRepository>(factory.CreateUserRepository);
        _employeeRepository = new Lazy<IEmployeeRepository>(factory.CreateEmployeeRepository);
        _authorRepository = new Lazy<IAuthorRepository>(factory.CreateAuthorRepository);
        _genreRepository = new Lazy<IGenreRepository>(factory.CreateGenreRepository);
        _bookRepository = new Lazy<IBookRepository>(factory.CreateBookRepository);
        _borrowRepository = new Lazy<IBorrowRepository>(factory.CreateBorrowRepository);
        _bookReviewRepository = new Lazy<IBookReviewRepository>(factory.CreateBookReviewRepository);
        _wishlistRepository = new Lazy<IWishlistRepository>(factory.CreateWishlistRepository);
        _pictureRepository = new Lazy<IPictureRepository>(factory.CreatePictureRepository);
        _context = context;
    }

    public IUserRepository User => _userRepository.Value;

    public IEmployeeRepository Employee => _employeeRepository.Value;

    public IAuthorRepository Author => _authorRepository.Value;

    public IGenreRepository Genre => _genreRepository.Value;

    public IBookRepository Book => _bookRepository.Value;

    public IBorrowRepository Borrow => _borrowRepository.Value;

    public IBookReviewRepository BookReview => _bookReviewRepository.Value;

    public IWishlistRepository Wishlist => _wishlistRepository.Value;

    public IPictureRepository Picture => _pictureRepository.Value;

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