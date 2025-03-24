using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application.Core.Factories;

public class ValidatorFactory : IValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IValidator<T> Initiate<T>()
    {
        var validator = _serviceProvider.GetService<IValidator<T>>() ?? throw new Exception($"Validator for type {typeof(T)} not found.");

        return validator;
    }
}