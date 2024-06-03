namespace LibrarySystem.Domain;

public class DeserializationException : Exception
{
    public DeserializationException() : base("Failed to deserialize the response content.")
    {
    }
}
