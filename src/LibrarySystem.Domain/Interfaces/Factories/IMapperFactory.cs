using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Domain.Interfaces.Factories;

public interface IMapperFactory
{
    IMapper<Author, CreateAuthorDto> CreateAuthorMapper();
    IMapper<Book, CreateBookDto> CreateBookMapper();
    IMapper<BookReview, CreateBookReviewDto> CreateBookReviewMapper();
    IMapper<Borrow, CreateBorrowDto> CreateBorrowMapper();
    IMapper<Employee, RegisterEmployeeDto> CreateEmployeeMapper();
    IMapper<Genre, CreateGenreDto> CreateGenreMapper();
    IMapper<User, RegisterUserDto> CreateUserMapper();
    IMapper<Wishlist, CreateWishlistDto> CreateWishlistMapper();
}
