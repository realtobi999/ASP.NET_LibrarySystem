using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts;

public interface IBookAssociations
{
    public Task HandleAuthorsAsync(IEnumerable<Guid> authorIds, Book book);
    public Task HandleGenresAsync(IEnumerable<Guid> genreIds, Book book);
}
