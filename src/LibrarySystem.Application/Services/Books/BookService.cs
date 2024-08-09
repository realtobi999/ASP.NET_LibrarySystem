using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;

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
            PublishedDate = createBookDto.PublishedDate,
        };

        var availability = createBookDto.Available;
        if (availability is not null)
        {
            book.IsAvailable = (bool)availability;
        }

        var authorIds = createBookDto.AuthorIds ?? throw new NullReferenceException("Atleast one author must be assigned.");
        var genreIds = createBookDto.GenreIds ?? throw new NullReferenceException("Atleast one genre must be assigned.");

        // handle authors
        await _associations.AssignAuthors(authorIds, book);

        // handle genres
        await _associations.AssignGenres(genreIds, book);

        // create book and save changes
        _repository.Book.Create(book);
        await _repository.SaveAsync();

        return book;
    }

    public async Task<int> Delete(Guid id)
    {
        var book = await this.Get(id);

        _associations.CleanAuthors(book);
        _associations.CleanGenres(book);

        _repository.Book.Delete(book);
        return await _repository.SaveAsync();
    }

    public async Task<Book> Get(Guid id, bool withRelations = true)
    {
        var book = await _repository.Book.Get(id, withRelations) ?? throw new NotFound404Exception(nameof(Book), id);

        return book;
    }

    public async Task<IEnumerable<Book>> GetAll(bool withRelations = true)
    {
        var books = await _repository.Book.GetAll(withRelations);

        return books;
    }

    public async Task<Book> Get(string isbn, bool withRelations = true)
    {
        var book = await _repository.Book.Get(isbn, withRelations) ?? throw new NotFound404Exception(nameof(Book), $"ISBN {isbn}"); 

        return book;
    }

    public async Task<int> Update(Guid id, UpdateBookDto updateBookDto)
    {
        var book = await this.Get(id);

        var title = updateBookDto.Title;
        var availability = updateBookDto.Availability;
        var description = updateBookDto.Description;
        var pages = updateBookDto.PagesCount;
        var published = updateBookDto.PublishedDate;
        var authors = updateBookDto.Authors;
        var genres = updateBookDto.Genres;

        book.Title = title;
        book.Description = description;
        book.PagesCount = pages;
        book.PublishedDate = published;

        if (availability is not null)
        {
            book.IsAvailable = (bool)availability;
        }
        if (authors is not null)
        {
            _associations.CleanAuthors(book);

            // Handle authors
            await _associations.AssignAuthors(authors.Select(a => a.Id), book);
        }
        if (genres is not null)
        {
            _associations.CleanGenres(book);

            // Handle genres
            await _associations.AssignGenres(genres.Select(g => g.Id), book);
        }

        return await _repository.SaveAsync();
    }

    public async Task<int> SetAvailable(Book book)
    {
        book.IsAvailable = true;

        return await _repository.SaveAsync();
    }

    public async Task<int> SetAvailability(Book book, bool availability)
    {
        book.IsAvailable = availability;

        return await _repository.SaveAsync();
    }
}
