namespace LibrarySystem.Domain.Exceptions.NotFound;

public class BorrowNotFoundException(Guid Id) : NotFoundException($"The borrow with: {Id} doesnt exist.")
{

}
