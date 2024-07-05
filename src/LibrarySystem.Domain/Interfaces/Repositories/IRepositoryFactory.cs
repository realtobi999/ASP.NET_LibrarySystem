namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IRepositoryFactory
{
    IUserRepository CreateUserRepository();
    IEmployeeRepository CreateEmployeeRepository();
    IAuthorRepository CreateAuthorRepository();
    IGenreRepository CreateGenreRepository();
    IBookRepository CreateBookRepository();
    IAssociationsRepository CreateAssociationsRepository();
    IBorrowRepository CreateBorrowRepository();
    IBookReviewRepository CreateBookReviewRepository();
    IWishlistRepository CreateWishlistRepository();
    IBaseRepository CreateBaseRepository();
}
