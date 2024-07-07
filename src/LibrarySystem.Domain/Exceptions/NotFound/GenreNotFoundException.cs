namespace LibrarySystem.Domain.Exceptions.NotFound;

public class GenreNotFoundException(Guid Id) : NotFoundException($"The genre with: {Id} doesnt exist.")
{

}
