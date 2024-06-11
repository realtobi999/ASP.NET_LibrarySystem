using LibrarySystem.Application.Contracts.Services;

namespace LibrarySystem.Application.Contracts;

public interface IServiceFactory
{
    IUserService CreateUserService();
    IEmployeeService CreateEmployeeService();
    IAuthorService CreateAuthorService();
    IGenreService CreateGenreService();
    IBookService CreateBookService();
    IBorrowService CreateBorrowService();
}
