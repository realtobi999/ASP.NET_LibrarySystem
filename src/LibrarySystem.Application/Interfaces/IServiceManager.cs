using LibrarySystem.Application.Interfaces.Services;

namespace LibrarySystem.Application.Interfaces;

public interface IServiceManager
{
    IUserService User { get; }
    IEmployeeService Employee { get; }
    IAuthorService Author { get; }
    IGenreService Genre { get; }
    IBookService Book { get; }
    IBorrowService Borrow { get; }
    IBookReviewService BookReview { get; }
    IWishlistService Wishlist { get; }
    IPictureService Picture  { get; }
}
