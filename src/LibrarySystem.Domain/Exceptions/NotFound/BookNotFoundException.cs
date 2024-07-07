namespace LibrarySystem.Domain.Exceptions.NotFound;

public class BookNotFoundException(Guid Id) : NotFoundException($"The book with id: {Id} doesnt exist.")
{

}