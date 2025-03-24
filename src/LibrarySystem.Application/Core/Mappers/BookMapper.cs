using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;

namespace LibrarySystem.Application.Core.Mappers;

public class BookMapper : IMapper<Book, CreateBookDto>
{
    private readonly IRepositoryManager _repository;

    public BookMapper(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public Book Map(CreateBookDto dto)
    {
        var book = new Book
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Isbn = dto.Isbn,
            Title = dto.Title,
            Description = dto.Description,
            PagesCount = dto.PagesCount,
            PublishedDate = dto.PublishedDate,
            Popularity = Book.POPULARITY_DEFAULT_VALUE,
            IsAvailable = (bool)dto.Available!, // property is marked by the required attribute in the dto
            CreatedAt = DateTimeOffset.UtcNow
        };

        // assign the genre and authors
        foreach (var genreId in dto.GenreIds)
        {
            var genre = _repository.Genre.Get(genreId);

            if (genre is not null)
            {
                book.Genres.Add(genre);
            }
        }

        foreach (var authorId in dto.AuthorIds)
        {
            var author = _repository.Author.Get(authorId);

            if (author is not null)
            {
                book.Authors.Add(author);
            }
        }

        return book;
    }
}