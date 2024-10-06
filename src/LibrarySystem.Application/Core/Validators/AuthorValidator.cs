using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Application.Core.Validators;

public class AuthorValidator : IValidator<Author>
{
    public Task<ValidationResult> ValidateAsync(Author entity)
    {
        throw new NotImplementedException();
    }
}
