
namespace LibrarySystem.Domain.Exceptions;


public class ZeroRowsAffectedException : InternalServerErrorException
{
    public ZeroRowsAffectedException() : base("Zero affected rows while trying to modify the database.") 
    {
    }
}
