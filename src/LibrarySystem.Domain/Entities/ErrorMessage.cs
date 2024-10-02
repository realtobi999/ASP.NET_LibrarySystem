namespace LibrarySystem.Domain.Entities;

/// <summary>
/// Represents an error message containing details about an error response.
/// </summary>
public class ErrorMessage
{
    /// <summary>
    /// Gets or sets the HTTP status code associated with the error.
    /// </summary>
    public int StatusCode { get; init; }

    /// <summary>
    /// Gets or sets the type of the error, which can provide additional context.
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// Gets or sets the title of the error, usually a short summary of the problem.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Gets or sets the detailed description of the error.
    /// </summary>
    public string? Detail { get; init; }

    /// <summary>
    /// Gets or sets a URI reference that identifies the specific occurrence of the error.
    /// </summary>
    public string? Instance { get; init; }
}
