using LibrarySystem.Application;
using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation;

[ApiController]
/*

GET     /api/employee params: offset, limit
GET     /api/employee/{employee_id}
PUT     /api/employee/{employee_id}
DELETE  /api/employee/{employee_id}

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
        var employees = await _service.Employee.GetAll();

        if (offset > 0)
            employees = employees.Skip(offset);
        if (limit > 0)
            employees = employees.Take(limit); 

        return Ok(employees);
    }

    [HttpGet("api/employee/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployee(Guid employeeId)
    {
        var employee = await _service.Employee.Get(employeeId);

        return Ok(employee);
    }

    [Authorize(Policy = "Employee"), EmployeeAuth]
    [HttpPut("api/employee/{employeeId:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid employeeId, [FromBody] UpdateEmployeeDto updateEmployeeDto)
    {
        var affected = await _service.Employee.Update(employeeId, updateEmployeeDto);
        if (affected == 0)
        {
            throw new InternalServerErrorException("Zero affected rows while trying to modify the database.");
        }

        return Ok();
    }

    [Authorize(Policy = "Employee"), EmployeeAuth]
    [HttpDelete("api/employee/{employeeId:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid employeeId)
    {
        var affected = await _service.Employee.Delete(employeeId);
        if (affected == 0)
        {
            throw new InternalServerErrorException("Zero affected rows while trying to modify the database.");
        }

        return Ok();
    }

}
