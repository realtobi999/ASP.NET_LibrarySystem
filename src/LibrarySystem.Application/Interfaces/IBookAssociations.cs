using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces;

public interface IBookAssociations // TODO: přehodit do Services.Books 
{
    public Task AssignAuthorsAsync(IEnumerable<Guid> authorIds, Book book);
    public Task AssignGenresAsync(IEnumerable<Guid> genreIds, Book book);
    public void CleanAuthors(Book book);
    public void CleanGenres(Book book);
}
