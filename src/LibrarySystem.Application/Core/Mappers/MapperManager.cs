using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class MapperManager : IMapperManager
{
    private readonly IMapperFactory _factory;

    private readonly Lazy<IMapper<Author, CreateAuthorDto>> _authorMapper;
    private readonly Lazy<IMapper<Book, CreateBookDto>> _bookMapper;
    private readonly Lazy<IMapper<BookReview, CreateBookReviewDto>> _bookReviewMapper;
    private readonly Lazy<IMapper<Borrow, CreateBorrowDto>> _borrowMapper;
    private readonly Lazy<IMapper<Employee, RegisterEmployeeDto>> _employeeMapper;
    private readonly Lazy<IMapper<Genre, CreateGenreDto>> _genreMapper;
    private readonly Lazy<IMapper<User, RegisterUserDto>> _userMapper;
    private readonly Lazy<IMapper<Wishlist, CreateWishlistDto>> _wishlistMapper;

    public MapperManager(IMapperFactory factory)
    {
        _factory = factory;

        _authorMapper = new(() => _factory.Initiate<Author, CreateAuthorDto>());
        _bookMapper = new(() => _factory.Initiate<Book, CreateBookDto>());
        _bookReviewMapper = new(() => _factory.Initiate<BookReview, CreateBookReviewDto>());
        _borrowMapper = new(() => _factory.Initiate<Borrow, CreateBorrowDto>());
        _employeeMapper = new(() => _factory.Initiate<Employee, RegisterEmployeeDto>());
        _genreMapper = new(() => _factory.Initiate<Genre, CreateGenreDto>());
        _userMapper = new(() => _factory.Initiate<User, RegisterUserDto>());
        _wishlistMapper = new(() => _factory.Initiate<Wishlist, CreateWishlistDto>());
    }

    public IMapper<Author, CreateAuthorDto> Author => _authorMapper.Value;
    public IMapper<Book, CreateBookDto> Book => _bookMapper.Value;
    public IMapper<BookReview, CreateBookReviewDto> BookReview => _bookReviewMapper.Value;
    public IMapper<Borrow, CreateBorrowDto> Borrow => _borrowMapper.Value;
    public IMapper<Employee, RegisterEmployeeDto> Employee => _employeeMapper.Value;
    public IMapper<Genre, CreateGenreDto> Genre => _genreMapper.Value;
    public IMapper<User, RegisterUserDto> User => _userMapper.Value;
    public IMapper<Wishlist, CreateWishlistDto> Wishlist => _wishlistMapper.Value;
}
