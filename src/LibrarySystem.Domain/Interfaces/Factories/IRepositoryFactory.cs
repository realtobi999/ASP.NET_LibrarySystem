using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Domain.Interfaces.Factories;

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
    IPictureRepository CreatePictureRepository();
    IBaseRepository CreateBaseRepository();
}
