using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IBookService
{
    Task<Book> Get(Guid id);
    Task<Book> Create(CreateBookDto createBookDto);
}
