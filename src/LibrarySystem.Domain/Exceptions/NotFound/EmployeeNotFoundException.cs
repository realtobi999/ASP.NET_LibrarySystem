using LibrarySystem.Domain.Exceptions;

namespace LibrarySystem.Domain;

public class EmployeeNotFoundException(Guid Id) : NotFoundException($"The employee with: {Id} doesnt exist.")
{

}
