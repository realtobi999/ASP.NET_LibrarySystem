using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Factories;

namespace LibrarySystem.Application.Core.Factories;

public class ValidatorFactory : IValidatorFactory
{
    public IValidator<T> CreateValidator<T>()
    {
        throw new NotImplementedException();
    }
}
