using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Domain.Interfaces.Managers;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IEmployeeRepository Employee { get; }
    IAuthorRepository Author { get; }
    IGenreRepository Genre { get; }
    IBookRepository Book { get; }
    IBorrowRepository Borrow { get; }
    IBookReviewRepository BookReview { get; }
    IWishlistRepository Wishlist { get; }
    IPictureRepository Picture { get; }
    Task<int> SaveAsync();

    /// <summary>
    /// Performs a check of how many rows were affected, if zero throws an <c>ZeroRowsAffectedException</c>.
    /// </summary>
    Task SaveSafelyAsync();
}