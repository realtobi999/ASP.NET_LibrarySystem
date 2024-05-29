using LibrarySystem.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation;

[ApiController]
/*

GET     /api/Employee params: offset, limit

*/
public class EmployeeController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeeController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("api/Employee")]
    public async Task<IActionResult> GetEmployees(int limit, int offset)
    {
        var Employee = await _service.EmployeeService.GetAll();

        if (offset > 0)
            Employee = Employee.Skip(offset);
        if (limit > 0)
            Employee = Employee.Take(limit); 

        return Ok(Employee);
    }
}
