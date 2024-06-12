using LibrarySystem.Domain.Exceptions;

namespace LibrarySystem.Domain.Exceptions;

public class UserNotFoundException(Guid Id) : NotFoundException($"The user with: {Id} doesnt exist.")
{

}
