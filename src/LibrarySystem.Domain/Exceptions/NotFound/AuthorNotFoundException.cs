namespace LibrarySystem.Domain.Exceptions.NotFound;

public class AuthorNotFoundException(Guid Id) : NotFoundException($"The author with id: {Id} doesnt exist.")
{

}
