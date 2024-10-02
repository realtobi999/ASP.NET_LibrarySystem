using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Services.Books;

public interface IBookAssociations
{
    public void AssignGenres(IEnumerable<Guid> genreIds, Book book);
    public void AssignAuthors(IEnumerable<Guid> authorIds, Book book);
    public void CleanAuthors(Book book);
    public void CleanGenres(Book book);
}
