namespace LibrarySystem.Domain.Exceptions;

public class GenreNotFoundException(Guid Id) : NotFoundException($"The genre with: {Id} doesnt exist.")
{

}
