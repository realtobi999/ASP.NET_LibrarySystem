using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAll(bool withRelations = true);
    Task<Book> Get(Guid id, bool withRelations = true);
    Task<Book> Get(string isbn, bool withRelations = true);
    Task<Book> Create(CreateBookDto createBookDto);
    Task<int> Update(Guid id, UpdateBookDto updateBookDto);
    Task<int> SetCoverPictures(Guid id, IEnumerable<byte[]> pictures);
    Task<int> SetAvailable(Book book); 
    Task<int> SetAvailability(Book book, bool availability);
    Task<int> Delete(Guid id);
}
