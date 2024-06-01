using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public class UpdateEmployeeDto
{
    public string? Name { get; set; }

    public string? Email { get; set; }
}
