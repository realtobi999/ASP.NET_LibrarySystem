namespace LibrarySystem.Domain;

public class NotAuthorizedException(string message) : Exception(message)
{

}
