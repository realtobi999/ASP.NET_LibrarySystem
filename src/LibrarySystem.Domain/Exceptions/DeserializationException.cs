namespace LibrarySystem.Domain.Exceptions;

public class DeserializationException : Exception
{
    public DeserializationException() : base("Failed to deserialize the response content.")
    {
    }
}
