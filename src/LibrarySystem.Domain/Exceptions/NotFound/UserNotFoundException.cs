namespace LibrarySystem.Domain.Exceptions.NotFound;

public class UserNotFoundException(Guid Id) : NotFoundException($"The user with: {Id} doesnt exist.")
{

}
