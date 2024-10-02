using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Interfaces.Factories;

public interface IValidatorFactory
{
    IValidator<T> CreateValidator<T>();
}
