namespace LibrarySystem.Domain.Entities;

public class ValidationResult
{
    public bool IsValid { get; init; }
    public Exception? Exception { get; init; }
}
