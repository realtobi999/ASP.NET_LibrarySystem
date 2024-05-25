using LibrarySystem.Domain.Exceptions;

namespace LibrarySystem.Domain;

public class UserNotFoundException(Guid Id) : NotFoundException($"The user {Id} doesnt exist.")
{

}
