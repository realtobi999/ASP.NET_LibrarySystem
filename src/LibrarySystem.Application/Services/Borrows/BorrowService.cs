using System.Security.Claims;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Dtos.Borrows;
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

    public async Task<Borrow> Create(CreateBorrowDto createBorrowDto)
    {
        var book = await _repository.Book.Get(createBorrowDto.BookId) ?? throw new NotFound404Exception(nameof(Book), createBorrowDto.BookId);
        if (!book.IsAvailable) // if the book is already borrowed throw an exception
        {
            throw new Conflict409Exception($"The book with id: {book.Id} is already borrowed.");
        }

        var user = await _repository.User.Get(createBorrowDto.UserId) ?? throw new NotFound404Exception(nameof(User), createBorrowDto.UserId);

        // create the new borrow instance
        var borrow = new Borrow
        {
            Id = createBorrowDto.Id ?? Guid.NewGuid(),
            BookId = book.Id,
            UserId = user.Id,
            BorrowDate = DateTimeOffset.UtcNow,
            DueDate = DateTimeOffset.UtcNow.AddMonths(1),
            IsReturned = false,
        };

        _repository.Borrow.Create(borrow);
        await _repository.SaveAsync();

        return borrow;
    }

    public async Task<Borrow> Get(Guid id)
    {
        var borrow = await _repository.Borrow.Get(id) ?? throw new NotFound404Exception(nameof(Borrow), id);

        return borrow;
    }

    public async Task<Borrow> Get(Guid bookId, Guid userId)
    {
        var borrow = await _repository.Borrow.Get(bookId, userId) ?? throw new NotFound404Exception(nameof(Borrow), $"bookId: {bookId}", $"userId: {userId}");

        return borrow;
    }

    public async Task<IEnumerable<Borrow>> Index()
    {
        var borrows = await _repository.Borrow.Index();

        return borrows;
    }

    public async Task<int> Return(Borrow borrow, string jwt)
    {
        var book = await _repository.Book.Get(borrow.BookId) ?? throw new NotFound404Exception(nameof(Book), borrow.BookId);

        return await this.Return(borrow, book, jwt);
    }

    public async Task<int> Return(Borrow borrow, Book book, string jwt)
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

        // set the book to available and the borrow status to returned
        book.IsAvailable = true;
        borrow.IsReturned = true;

        return await _repository.SaveAsync();
    }
}