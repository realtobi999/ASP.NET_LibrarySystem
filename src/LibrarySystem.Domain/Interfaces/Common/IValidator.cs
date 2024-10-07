namespace LibrarySystem.Domain.Interfaces.Common;


public interface IValidator<T>
{
    Task<(bool isValid, Exception? exception)> ValidateAsync(T entity);
}
