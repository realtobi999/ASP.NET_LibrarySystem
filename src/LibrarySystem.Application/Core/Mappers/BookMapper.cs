using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class BookMapper : IBookMapper
{
    private readonly IBookAssociations _associations;

    public BookMapper(IBookAssociations associations)
    {
        _associations = associations;
    }

    public Book CreateFromDto(CreateBookDto dto)
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

        // null check and assign authors and genres
        if (dto.AuthorIds is not null)
        {
            _associations.AssignAuthors(dto.AuthorIds, book);
        }
        if (dto.GenreIds is not null)
        {
            _associations.AssignGenres(dto.GenreIds, book);
        }

        return book;
    }

    public void UpdateFromDto(Book book, UpdateBookDto dto)
    {
        book.Title = dto.Title;
        book.Description = dto.Description;
        book.PagesCount = dto.PagesCount;
        book.PublishedDate = dto.PublishedDate;

        if (dto.Availability is not null)
        {
            book.IsAvailable = (bool)dto.Availability;
        }
        // check if the update request authors,genres property is null, if not clean the previous and assign the new
        if (dto.AuthorIds is not null)
        {
            _associations.CleanAuthors(book);

            _associations.AssignAuthors(dto.AuthorIds, book);
        }
        if (dto.GenreIds is not null)
        {
            _associations.CleanGenres(book);

            _associations.AssignGenres(dto.GenreIds, book);
        }
    }
}
