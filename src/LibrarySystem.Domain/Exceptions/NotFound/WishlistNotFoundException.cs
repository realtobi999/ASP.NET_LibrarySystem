namespace LibrarySystem.Domain.Exceptions.NotFound;

public class WishlistNotFoundException(Guid Id) : NotFoundException($"The wishlist with: {Id} doesnt exist.")
{

}