namespace LibrarySystem.Domain.Exceptions.NotFound;

public class EmployeeNotFoundException(Guid Id) : NotFoundException($"The employee with: {Id} doesnt exist.")
{

}
