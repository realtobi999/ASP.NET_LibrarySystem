using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services;
using LibrarySystem.Domain.Interfaces.Services.Books;

namespace LibrarySystem.Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthorService> _authorService;
    private readonly Lazy<IGenreService> _genreService;
    private readonly Lazy<IBookService> _bookService;
    private readonly Lazy<IBorrowService> _borrowService;
    private readonly Lazy<IBookReviewService> _bookReviewService;
    private readonly Lazy<IWishlistService> _wishlistService;
    private readonly Lazy<IPictureService> _pictureService;

    public ServiceManager(IServiceFactory factory)
    {
        _userService = new Lazy<IUserService>(factory.CreateUserService);
        _employeeService = new Lazy<IEmployeeService>(factory.CreateEmployeeService);
        _authorService = new Lazy<IAuthorService>(factory.CreateAuthorService);
        _genreService = new Lazy<IGenreService>(factory.CreateGenreService);
        _bookService = new Lazy<IBookService>(factory.CreateBookService);
        _borrowService = new Lazy<IBorrowService>(factory.CreateBorrowService);
        _bookReviewService = new Lazy<IBookReviewService>(factory.CreateBookReviewService);
        _wishlistService = new Lazy<IWishlistService>(factory.CreateWishlistService);
        _pictureService = new Lazy<IPictureService>(factory.CreatePictureService);
    }

    public IUserService User => _userService.Value;

    public IEmployeeService Employee => _employeeService.Value;

    public IAuthorService Author => _authorService.Value;

    public IGenreService Genre => _genreService.Value;

    public IBookService Book => _bookService.Value;

    public IBorrowService Borrow => _borrowService.Value;

    public IBookReviewService BookReview => _bookReviewService.Value;

    public IWishlistService Wishlist => _wishlistService.Value;

    public IPictureService Picture => _pictureService.Value;
}