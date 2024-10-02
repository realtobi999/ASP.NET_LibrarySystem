using LibrarySystem.Application.Core.Mappers;
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
using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Factories;

public class MapperFactory : IMapperFactory
{
    private readonly IHasher _hasher;

    public MapperFactory(IHasher hasher)
    {
        _hasher = hasher;
    }

    public IMapper<Author, CreateAuthorDto> CreateAuthorMapper()
    {
        return new AuthorMapper();
    }

    public IMapper<Book, CreateBookDto> CreateBookMapper()
    {
        return new BookMapper();
    }

    public IMapper<BookReview, CreateBookReviewDto> CreateBookReviewMapper()
    {
        return new BookReviewMapper();
    }

    public IMapper<Borrow, CreateBorrowDto> CreateBorrowMapper()
    {
        return new BorrowMapper();
    }

    public IMapper<Employee, RegisterEmployeeDto> CreateEmployeeMapper()
    {
        return new EmployeeMapper(_hasher);
    }

    public IMapper<Genre, CreateGenreDto> CreateGenreMapper()
    {
        return new GenreMapper();
    }

    public IMapper<User, RegisterUserDto> CreateUserMapper()
    {
        return new UserMapper(_hasher);
    }

    public IMapper<Wishlist, CreateWishlistDto> CreateWishlistMapper()
    {
        return new WishlistMapper();
    }
}
