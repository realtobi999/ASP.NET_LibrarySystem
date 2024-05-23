namespace LibrarySystem.Domain.Exceptions;

public class InternalServerErrorException(string message) : Exception(message)
{

}