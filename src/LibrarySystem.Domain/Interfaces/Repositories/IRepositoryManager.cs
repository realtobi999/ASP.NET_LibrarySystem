namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IEmployeeRepository Employee { get; }
    IAuthorRepository Author { get; }
    IGenreRepository Genre { get; }
    IAssociationsRepository Associations { get; }
    IBookRepository Book { get; }
    IBorrowRepository Borrow { get; }
    IBookReviewRepository BookReview { get; }
    IWishlistRepository Wishlist { get ;}
    Task<int> SaveAsync(); 
}
