namespace LibrarySystem.Domain.Exceptions.NotFound;

public class BookReviewNotFoundException(Guid Id) : NotFoundException($"The review with: {Id} doesnt exist.")
{

}