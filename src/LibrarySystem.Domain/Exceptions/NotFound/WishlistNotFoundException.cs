namespace LibrarySystem.Domain.Exceptions;

public class WishlistNotFoundException(Guid Id) : NotFoundException($"The wishlist with: {Id} doesnt exist.")
{

}