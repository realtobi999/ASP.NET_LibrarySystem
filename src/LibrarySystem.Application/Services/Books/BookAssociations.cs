using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Books;

public class BookAssociations : IBookAssociations 
{
    private readonly IRepositoryManager _repository;

    public BookAssociations(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task AssignAuthors(IEnumerable<Guid> authorIds, Book book)
    {
        var tasks = authorIds.Select(async authorId =>
        {
            var author = await _repository.Author.Get(authorId) ?? throw new NotFound404Exception(nameof(Author), authorId);
            _repository.Associations.CreateBookAuthor(new BookAuthor
            {
                BookId = book.Id,
                Book = book,
                AuthorId = author.Id,
                Author = author
            });
        });

        await Task.WhenAll(tasks);
    }

    public async Task AssignGenres(IEnumerable<Guid> genreIds, Book book)
    {
        var tasks = genreIds.Select(async genreId =>
        {
            var genre = await _repository.Genre.Get(genreId) ?? throw new NotFound404Exception(nameof(Genre), genreId);
            _repository.Associations.CreateBookGenre(new BookGenre
            {
                BookId = book.Id,
                Book = book,
                GenreId = genre.Id,
                Genre = genre,
            });
        });

        await Task.WhenAll(tasks);
    }

    public void CleanAuthors(Book book)
    {
        foreach (var association in book.BookAuthors)
        {
            _repository.Associations.RemoveBookAuthor(association);
        }
    }

    public void CleanGenres(Book book)
    {
        foreach (var association in book.BookGenres)
        {
            _repository.Associations.RemoveBookGenre(association);
        }
    }
}
