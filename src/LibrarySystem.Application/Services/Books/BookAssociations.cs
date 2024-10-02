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

    public void AssignAuthors(IEnumerable<Guid> authorIds, Book book)
    {
        foreach (var authorId in authorIds)
        {
            var author = _repository.Author.Get(authorId) ?? throw new NotFound404Exception(nameof(Author), authorId);
            _repository.Associations.CreateBookAuthor(new BookAuthor
            {
                BookId = book.Id,
                Book = book,
                AuthorId = author.Id,
                Author = author
            });
        }
    }

    public void AssignGenres(IEnumerable<Guid> genreIds, Book book)
    {
        foreach (var genreId in genreIds)
        {
            var genre = _repository.Genre.Get(genreId) ?? throw new NotFound404Exception(nameof(Genre), genreId);
            _repository.Associations.CreateBookGenre(new BookGenre
            {
                BookId = book.Id,
                Book = book,
                GenreId = genre.Id,
                Genre = genre,
            });
        }
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
