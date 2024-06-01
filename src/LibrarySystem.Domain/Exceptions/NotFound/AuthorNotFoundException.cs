namespace LibrarySystem.Domain.Exceptions;

public class AuthorNotFoundException(Guid Id) : NotFoundException($"The author with id: {Id} doesnt exist.")
{

}
