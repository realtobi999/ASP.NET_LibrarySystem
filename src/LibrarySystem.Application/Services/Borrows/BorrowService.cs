using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain.Dtos.Borrows;
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
            throw new ConflictException($"The book with id: {book.Id} is already borrowed.");
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
            Returned = false,
        };

        // set the availability of the book to false -> borrowed
        book.Available = false;

        _repository.Borrow.Create(borrow);
        await _repository.SaveAsync();

        return borrow;
    }

    public async Task<Borrow> Get(Guid id)
    {
        var borrow = await _repository.Borrow.Get(id) ?? throw new BorrowNotFoundException(id);

        return borrow;
    }

    public async Task<Borrow> Get(Guid bookId, Guid userId)
    {
        var borrow = await _repository.Borrow.Get(bookId, userId) ?? throw new NotFoundException("The book with these ID's doesnt exist.");

        return borrow;
    }

    public async Task<IEnumerable<Borrow>> GetAll()
    {
        var borrows = await _repository.Borrow.GetAll();

        return borrows;
    }

    public async Task<int> Return(Borrow borrow)
    {
        var book = await _repository.Book.Get(borrow.BookId) ?? throw new BookNotFoundException(borrow.BookId);

        if (book.Available) // if the book isn't borrowed throw an exception
        {
            throw new ConflictException($"The book with ID: {book.Id} is not currently borrowed. Please check the book ID and try again.");
        }

        if (borrow.Returned)
        {
            throw new BadRequestException($"The borrow record for book ID: {book.Id} is already closed. This book has already been returned.");
        }

        // check if it is past the due date
        if (DateTimeOffset.UtcNow > borrow.BorrowDue)
        {
            throw new ConflictException($"The book with ID: {book.Id} cannot be returned because it is past the due date ({borrow.BorrowDue}). Please contact the library for assistance.");
        }

        // set the availability of the book back to true -> free to borrow
        book.Available = true;
        borrow.Returned = true;

        return await _repository.SaveAsync();
    }
}