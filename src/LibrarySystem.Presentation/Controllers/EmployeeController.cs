using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Exceptions.Common;
using LibrarySystem.Domain.Interfaces.Managers;
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
        var employees = await _service.Employee.Index();

        return Ok(employees.Paginate(offset, limit));
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
            throw new ZeroRowsAffectedException();
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
            throw new ZeroRowsAffectedException();
        }

        return Ok();
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/employee/{employeeId:guid}/photos")]
    public async Task<IActionResult> UploadPhotos(Guid employeeId, IFormFile file)
    {
        var picture = await _service.Picture.Extract(file);
        var employee = await _service.Employee.Get(employeeId); // validates if the employee exists

        // delete any previous associated photo
        _repository.Picture.DeleteWhere(p => p.EntityId == employee.Id && p.EntityType == PictureEntityType.Employee);

        // assign the id to the pictures and push them to the database
        var affected = await _service.Picture.CreateWithEntity(picture, employee.Id, PictureEntityType.Employee);

        if (affected == 0)
        {
            throw new ZeroRowsAffectedException();
        }

        return Ok();
    }
}
