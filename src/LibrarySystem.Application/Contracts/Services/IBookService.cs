using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAll();
    Task<Book> Get(Guid id);
    Task<Book> Create(CreateBookDto createBookDto);
}
