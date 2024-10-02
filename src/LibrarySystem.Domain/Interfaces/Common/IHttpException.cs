namespace LibrarySystem.Domain.Interfaces.Common;

/// <summary>
/// Represents an HTTP exception with status code, title, and message details.
/// </summary>
public interface IHttpException
{
    /// <summary>
    /// Gets the HTTP status code associated with the exception.
    /// </summary>
    int StatusCode { get; }

    /// <summary>
    /// Gets the title of the exception.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets the detailed message of the exception.
    /// </summary>
    string Message { get; }
}
