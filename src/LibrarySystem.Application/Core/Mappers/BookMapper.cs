using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class BookMapper : IMapper<Book, CreateBookDto>
{
    public Book Map(CreateBookDto dto)
    {
        var book = new Book
        {
            Id = dto.Id ?? Guid.NewGuid(),
            ISBN = dto.ISBN,
            Title = dto.Title,
            Description = dto.Description,
            PagesCount = dto.PagesCount,
            PublishedDate = dto.PublishedDate,
        };

        // assign the genre and authors
        foreach (var genreId in dto.GenreIds)
        {
            book.BookGenres.Add(new BookGenre
            {
                BookId = book.Id,
                GenreId = genreId,
            });
        }
        foreach (var authorId in dto.AuthorIds)
        {
            book.BookAuthors.Add(new BookAuthor
            {
                BookId = book.Id,
                AuthorId = authorId,
            });
        }

        return book;
    }
}
