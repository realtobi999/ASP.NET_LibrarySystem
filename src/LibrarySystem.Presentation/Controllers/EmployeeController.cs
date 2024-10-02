using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Mappers;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/employee params: offset, limit
GET     /api/employee/{employee_id}
PUT     /api/employee/{employee_id}
PUT     /api/employee/{employee_id}/photos
DELETE  /api/employee/{employee_id}

*/
public class EmployeeController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRepositoryManager _repository;

    public EmployeeController(IServiceManager service, IRepositoryManager repository)
    {
        _service = service;
        _repository = repository;
    }

    [HttpGet("api/employee")]
    public async Task<IActionResult> GetEmployees(int limit, int offset)
    {
        var employees = await _service.Employee.IndexAsync();

        return Ok(employees.Paginate(offset, limit));
    }

    [HttpGet("api/employee/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployee(Guid employeeId)
    {
        var employee = await _service.Employee.GetAsync(employeeId);

        return Ok(employee);
    }

    [Authorize(Policy = "Employee"), EmployeeAuth]
    [HttpPut("api/employee/{employeeId:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid employeeId, [FromBody] UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _service.Employee.GetAsync(employeeId);

        employee.Update(updateEmployeeDto);
        await _service.Employee.UpdateAsync(employee);

        return NoContent();
    }

    [Authorize(Policy = "Employee"), EmployeeAuth]
    [HttpDelete("api/employee/{employeeId:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid employeeId)
    {
        var employee = await _service.Employee.GetAsync(employeeId);

        await _service.Employee.DeleteAsync(employee);

        return NoContent();
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/employee/{employeeId:guid}/photos")]
    public async Task<IActionResult> UploadPhotos(Guid employeeId, IFormFile file)
    {
        var picture = await _service.Picture.Extract(file);
        var employee = await _service.Employee.GetAsync(employeeId); // validates if the employee exists

        // delete any previous associated photo
        _repository.Picture.DeleteWhere(p => p.EntityId == employee.Id && p.EntityType == PictureEntityType.Employee);

        // assign the id to the pictures and push them to the database
        await _service.Picture.CreateWithEntity(picture, employee.Id, PictureEntityType.Employee);

        return NoContent();
    }
}
