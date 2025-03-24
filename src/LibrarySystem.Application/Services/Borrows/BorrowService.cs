using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Borrows;

public class BorrowService : IBorrowService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Borrow> _validator;

    public BorrowService(IRepositoryManager repository, IValidator<Borrow> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task CreateAsync(Borrow borrow)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(borrow);
        if (!valid && exception is not null) throw exception;

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

    public async Task UpdateAsync(Borrow borrow)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(borrow);
        if (!valid && exception is not null) throw exception;

        // delete borrow entity and save changes
        _repository.Borrow.Update(borrow);
        await _repository.SaveSafelyAsync();
    }

    public async Task UpdateIsReturnedAsync(Borrow borrow, bool isReturned)
    {
        borrow.UpdateIsReturned(isReturned);

        await this.UpdateAsync(borrow);
    }
}