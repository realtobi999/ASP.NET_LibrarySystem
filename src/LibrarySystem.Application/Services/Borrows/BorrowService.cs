using System.Security.Claims;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Borrows;

public class BorrowService : IBorrowService
{
    private readonly IRepositoryManager _repository;

    public BorrowService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(Borrow borrow)
    {
        // create borrow entity and save changes
        _repository.Borrow.Create(borrow);
        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Borrow borrow)
    {
        // delete borrow entity and save changes
        _repository.Borrow.Delete(borrow);
        await _repository.SaveSafelyAsync();
    }

    public async Task<Borrow> GetAsync(Guid id)
    {
        var borrow = await _repository.Borrow.GetAsync(id) ?? throw new NotFound404Exception(nameof(Borrow), id);

        return borrow;
    }

    public async Task<Borrow> GetAsync(Guid bookId, Guid userId)
    {
        var borrow = await _repository.Borrow.GetAsync(bookId, userId) ?? throw new NotFound404Exception(nameof(Borrow), $"bookId: {bookId}", $"userId: {userId}");

        return borrow;
    }

    public async Task<IEnumerable<Borrow>> IndexAsync()
    {
        var borrows = await _repository.Borrow.IndexAsync();

        return borrows;
    }

    public async Task ReturnAsync(Borrow borrow, string jwt, Func<Book, Task> updateBookAsync)
    {
        var book = await _repository.Book.GetAsync(borrow.BookId) ?? throw new NotFound404Exception(nameof(Book), borrow.BookId);

        await this.ReturnAsync(borrow, book, jwt, updateBookAsync);
    }

    public async Task ReturnAsync(Borrow borrow, Book book, string jwt, Func<Book, Task> UpdateBookAsync)
    {
        var role = JwtUtils.ParseFromPayload(jwt, ClaimTypes.Role);

        if (borrow.IsReturned)
        {
            throw new BadRequest400Exception($"The borrow record for book ID: {book.Id} is already closed. This book has already been IsReturned.");
        }
        if (book.IsAvailable)
        {
            throw new Conflict409Exception($"The book with ID: {book.Id} is not currently borrowed. Please check the book ID and try again.");
        }
        if (DateTimeOffset.UtcNow > borrow.DueDate && role != "Employee") // this role check enables the librarians to return the book even if it's past due
        {
            throw new Conflict409Exception($"The book with ID: {book.Id} cannot be returned because it is past the due date ({borrow.DueDate}). Please contact the library for assistance.");
        }

        // set the book to available and the borrow status to returned, after that update both
        book.SetIsAvailable(true);
        borrow.SetIsReturned(true);

        // update both entities
        await UpdateAsync(borrow);
        await UpdateBookAsync(book);
    }

    public async Task UpdateAsync(Borrow borrow)
    {
        // delete borrow entity and save changes
        _repository.Borrow.Update(borrow);
        await _repository.SaveSafelyAsync();
    }
}