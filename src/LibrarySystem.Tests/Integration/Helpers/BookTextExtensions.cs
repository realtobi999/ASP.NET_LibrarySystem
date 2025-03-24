using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class BookTestExtensions
{
    public static CreateBookDto ToCreateBookDto(this Book book, IEnumerable<Guid> authorIds, IEnumerable<Guid> genreIds)
    {
        return new CreateBookDto
        {
            Id = book.Id,
            Isbn = book.Isbn,
            Title = book.Title,
            Description = book.Description,
            PagesCount = book.PagesCount,
            PublishedDate = book.PublishedDate,
            Available = book.IsAvailable,
            AuthorIds = authorIds,
            GenreIds = genreIds
        };
    }
}