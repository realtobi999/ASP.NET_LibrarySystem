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

    [HttpGet("api/employee")]
    public async Task<IActionResult> GetEmployees(int limit, int offset)
    {
        var employees = await _service.EmployeeService.GetAll();

        if (offset > 0)
            employees = employees.Skip(offset);
        if (limit > 0)
            employees = employees.Take(limit); 

        return Ok(employees);
    }

    [HttpGet("api/employee/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployee(Guid employeeId)
    {
        var employee = await _service.EmployeeService.Get(employeeId);

        return Ok(employee);
    }
}
