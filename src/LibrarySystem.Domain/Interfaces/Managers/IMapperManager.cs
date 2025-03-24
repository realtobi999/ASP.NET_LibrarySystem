using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Interfaces.Managers;

public interface IMapperManager
{
    IMapper<Author, CreateAuthorDto> Author { get; }
    IMapper<Book, CreateBookDto> Book { get; }
    IMapper<BookReview, CreateBookReviewDto> BookReview { get; }
    IMapper<Borrow, CreateBorrowDto> Borrow { get; }
    IMapper<Employee, RegisterEmployeeDto> Employee { get; }
    IMapper<Genre, CreateGenreDto> Genre { get; }
    IMapper<User, RegisterUserDto> User { get; }
    IMapper<Wishlist, CreateWishlistDto> Wishlist { get; }
}