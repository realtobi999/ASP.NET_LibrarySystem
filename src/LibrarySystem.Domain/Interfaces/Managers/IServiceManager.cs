using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Domain.Interfaces.Managers;

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
    IPictureService Picture { get; }
}
