using LibrarySystem.Application.Interfaces.Services;
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
        if (!book.IsAvailable) // if the book is already borrowed throw an exception
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
            DueDate = DateTimeOffset.UtcNow.AddMonths(1),
            IsReturned = false,
        };

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

    public async Task<int> SetIsReturned(Borrow borrow)
    {
        borrow.IsReturned = true;

        return await _repository.SaveAsync();
    }

    public async Task<int> SetIsReturned(Borrow borrow, bool returned)
    {
        borrow.IsReturned = returned;

        return await _repository.SaveAsync();
    }
}