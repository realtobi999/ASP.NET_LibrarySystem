using System.Security.Claims;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Responses;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Interfaces.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

POST    /api/auth/register
POST    /api/auth/login
POST    /api/auth/Employee/register
POST    /api/auth/Employee/login

*/
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IJwt _jwt;

    public AuthController(IServiceManager service, IJwt jwt)
    {
        _service = service;
        _jwt = jwt;
    }

    [HttpPost("api/auth/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        var user = await _service.User.Create(registerUserDto);

        return Created(string.Format("/api/user/{0}", user.Id), null);
    }

    [HttpPost("api/auth/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        var authorized = await _service.User.Login(loginUserDto);
        if (!authorized)
        {
            throw new NotAuthorizedException("These credentials are invalid.");
        }

        var user = await _service.User.Get(loginUserDto.Email!);
        var token = _jwt.Generate([
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User"),
        ]);

        return Ok(new LoginUserResponseDto
        {
            UserDto = user.ToDto(),
            Token = token,
        });
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("api/auth/employee/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterEmployeeDto registerEmployeeDto)
    {
        var employee = await _service.Employee.Create(registerEmployeeDto);

        return Created(string.Format("/api/employee/{0}", employee.Id), null);
    }


    [HttpPost("api/auth/employee/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginEmployeeDto loginEmployeeDto)
    {
        var authorized = await _service.Employee.Login(loginEmployeeDto);
        if (!authorized)
        {
            throw new NotAuthorizedException("These credentials are invalid.");
        }

        var employee = await _service.Employee.Get(loginEmployeeDto.Email!);
        var token = _jwt.Generate([
            new Claim("EmployeeId", employee.Id.ToString()),
            new Claim(ClaimTypes.Role, "Employee"),
        ]);

        return Ok(new LoginEmployeeResponseDto
        {
            EmployeeDto = employee.ToDto(),
            Token = token
        });
    }
}
