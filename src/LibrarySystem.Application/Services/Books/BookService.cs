using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
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
            PublishedDate = createBookDto.PublishedDate,
            IsAvailable = createBookDto.Available,
            CoverPicture = createBookDto.CoverPicture,
        };

        var authorIds = createBookDto.AuthorIds ?? throw new ArgumentNullException("Atleast one author must be assigned.");
        var genreIds = createBookDto.GenreIds ?? throw new ArgumentNullException("Atleast one genre must be assigned.");

        // handle authors
        await _associations.AssignAuthorsAsync(authorIds, book);

        // handle genres
        await _associations.AssignGenresAsync(genreIds, book);

        // create book and save changes
        _repository.Book.Create(book);
        await _repository.SaveAsync();

        return book;
    }

    public async Task<int> Delete(Guid id)
    {
        var book = await _repository.Book.Get(id) ?? throw new BookNotFoundException(id);

        _repository.Book.Delete(book);
        return await _repository.SaveAsync();
    }

    public async Task<Book> Get(Guid id, bool withRelations = true)
    {
        var book = await _repository.Book.Get(id, withRelations) ?? throw new BookNotFoundException(id);

        return book;
    }

    public async Task<IEnumerable<Book>> GetAll(bool withRelations = true)
    {
        var books = await _repository.Book.GetAll(withRelations);

        return books;
    }

    public async Task<Book> Get(string isbn, bool withRelations = true)
    {
        var book = await _repository.Book.Get(isbn, withRelations) ?? throw new NotFoundException($"The book with isbn: {isbn} doesnt exist.");

        return book; 
    }

    public async Task<int> Update(Guid id, UpdateBookDto updateBookDto)
    {
        var book = await _repository.Book.Get(id) ?? throw new BookNotFoundException(id);

        var title = updateBookDto.Title;
        var availability = updateBookDto.Availability;
        var description = updateBookDto.Description;
        var pages = updateBookDto.PagesCount;
        var published = updateBookDto.PublishedDate;
        var picture = updateBookDto.CoverPicture;
        var authors = updateBookDto.Authors;
        var genres = updateBookDto.Genres;

        if (!title.IsNullOrEmpty())
        {
            book.Title = title;
        }
        if (!description.IsNullOrEmpty())
        {
            book.Description = description;
        }
        if (pages > 0)
        {
            book.PagesCount = pages;
        }
        if (published != DateTimeOffset.MinValue)
        {
            book.PublishedDate = published;
        }

        if (!picture.IsNullOrEmpty())
        {
            book.CoverPicture = picture;
        }
        if (authors.Count != 0)
        {
            _associations.CleanAuthors(book);

            // Handle authors
            await _associations.AssignAuthorsAsync(authors.Select(a => a.Id), book);
        }
        if (genres.Count != 0)
        {
            _associations.CleanGenres(book);

            // Handle genres
            await _associations.AssignGenresAsync(genres.Select(g => g.Id), book);
        }
        if (availability != 0)
        {
            if (availability == 1) book.IsAvailable = true;
            if (availability == -1) book.IsAvailable = false;
        }

        return await _repository.SaveAsync();
    }

    public async Task<int> SetAvailable(Book book)
    {
        book.IsAvailable = true;
        
        return await _repository.SaveAsync();
    }

    public async Task<int> SetAvailable(Book book, bool availability)
    {
        book.IsAvailable = availability;
        
        return await _repository.SaveAsync();
    }
}
