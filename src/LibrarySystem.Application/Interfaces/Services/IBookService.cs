﻿using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAll(bool relations = true);
    Task<Book> Get(Guid id, bool relations = true);
    Task<Book> Get(string isbn, bool relations = true);
    Task<Book> Create(CreateBookDto createBookDto);
    Task<int> Update(Guid id, UpdateBookDto updateBookDto);
    Task<int> SetAvailable(Book book); 
    Task<int> SetAvailable(Book book, bool availability);
    Task<int> Delete(Guid id);
}
