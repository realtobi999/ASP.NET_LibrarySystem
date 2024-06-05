using LibrarySystem.Application.Contracts;
using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Services.Books;

public class BookService : IBookService
{
    private readonly IRepositoryManager _repository;
    private readonly IBookAssociations _associations; 

    public BookService(IRepositoryManager repository, IBookAssociations associations)
    {
        _repository = repository;
        _associations = associations;
    }

    public async Task<Book> Create(CreateBookDto createBookDto)
    {
        var book = new Book
        {
            Id = createBookDto.Id ?? Guid.NewGuid(),
            ISBN = createBookDto.ISBN,
            Title = createBookDto.Title,
            Description = createBookDto.Description,
            PagesCount = createBookDto.PagesCount,
            PublishedAt = createBookDto.PublishedAt,
            CoverPicture = createBookDto.CoverPicture,
        };

        var authorIds = createBookDto.AuthorIds ?? throw new ArgumentNullException("At least one author must be assigned.");
        var genreIds = createBookDto.GenreIds ?? throw new ArgumentNullException("At least one genre must be assigned.");

        // Handle authors
        await _associations.HandleAuthorsAsync(authorIds, book);

        // Handle genres
        await _associations.HandleGenresAsync(genreIds, book);        

        // Create book and save changes
        _repository.Book.Create(book);
        await _repository.SaveAsync();

        return book;
    }

    public async Task<Book> Get(Guid id)
    {
        var book = await _repository.Book.Get(id) ?? throw new BookNotFoundException(id);

        return book;
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        var books = await _repository.Book.GetAll();

        return books;
    }
}
