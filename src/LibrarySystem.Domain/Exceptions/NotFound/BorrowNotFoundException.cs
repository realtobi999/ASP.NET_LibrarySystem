namespace LibrarySystem.Domain.Exceptions;

public class BorrowNotFoundException(Guid Id) : NotFoundException($"The borrow with: {Id} doesnt exist.")
{

}
