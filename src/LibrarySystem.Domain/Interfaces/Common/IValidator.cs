using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Common;

/// <summary>
/// Defines functionality for validating an object of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object to validate.</typeparam>
public interface IValidator<T>
{
    /// <summary>
    /// Asynchronously validates an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A <see cref="ValidationResult"/> indicating the result of the validation.</returns>
    Task<ValidationResult> ValidateAsync(T entity);
}
