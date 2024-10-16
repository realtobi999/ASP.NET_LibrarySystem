using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Responses;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
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
    private readonly IMapperManager _mapper;

    public AuthController(IServiceManager service, IJwt jwt, IMapperManager mapper)
    {
        _service = service;
        _jwt = jwt;
        _mapper = mapper;
    }

    [HttpPost("api/auth/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        var user = _mapper.User.Map(registerUserDto);

        await _service.User.CreateAsync(user);

        return Created($"/api/user/{user.Id}", null);
    }

    [HttpPost("api/auth/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        // authorize the user, if fails return 401
        var authorized = await _service.User.AuthAsync(loginUserDto);
        if (!authorized)
        {
            throw new NotAuthorized401Exception();
        }

        // get the user by the email address and create their JWT
        var user = await _service.User.GetAsync(loginUserDto.Email!);
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
    public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeDto registerEmployeeDto)
    {
        var employee = _mapper.Employee.Map(registerEmployeeDto);

        await _service.Employee.CreateAsync(employee);

        return Created($"/api/employee/{employee.Id}", null);
    }

    [HttpPost("api/auth/employee/login")]
    public async Task<IActionResult> LoginEmployee([FromBody] LoginEmployeeDto loginEmployeeDto)
    {
        // authorize the employee, if fails return 401
        var authorized = await _service.Employee.AuthAsync(loginEmployeeDto);
        if (!authorized)
        {
            throw new NotAuthorized401Exception();
        }

        // get the employee by the email address and create their JWT
        var employee = await _service.Employee.GetAsync(loginEmployeeDto.Email!);
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
