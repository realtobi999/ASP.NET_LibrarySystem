namespace LibrarySystem.Domain.Exceptions;

public class BookNotFoundException(Guid Id) : NotFoundException($"The book with id: {Id} doesnt exist.")
{

}