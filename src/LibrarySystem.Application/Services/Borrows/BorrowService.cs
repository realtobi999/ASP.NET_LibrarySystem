using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;

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
        var book = await _repository.Book.Get(createBorrowDto.BookId) ?? throw new BookNotFoundException(createBorrowDto.BookId);
        if (!book.Available) // if the book is already borrowed throw an exception
        {
            throw new ConflictException($"The book with id: {book.Id} is already borrowed");
        }

        var user = await _repository.User.Get(createBorrowDto.UserId) ?? throw new UserNotFoundException(createBorrowDto.UserId);

        // create the new borrow instance
        var borrow = new Borrow
        {
            Id = createBorrowDto.Id ?? Guid.NewGuid(),
            BookId = book.Id,
            UserId = user.Id,
            BorrowDate = DateTimeOffset.UtcNow,
            BorrowDue = DateTimeOffset.UtcNow.AddMonths(1),
        };

        // set the availability of the book to false - borrowed
        book.Available = false; 

        _repository.Borrow.Create(borrow);
        await _repository.SaveAsync();

        return borrow;
    }
}