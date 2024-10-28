using LibrarySystem.Domain.Interfaces.Services;
using LibrarySystem.Domain.Interfaces.Services.Books;

namespace LibrarySystem.Domain.Interfaces.Factories;

public interface IServiceFactory
{
    IUserService CreateUserService();
    IEmployeeService CreateEmployeeService();
    IAuthorService CreateAuthorService();
    IGenreService CreateGenreService();
    IBookService CreateBookService();
    IBorrowService CreateBorrowService();
    IBookReviewService CreateBookReviewService();
    IWishlistService CreateWishlistService();
    IPictureService CreatePictureService();
}
