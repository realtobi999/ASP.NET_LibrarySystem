using System.Security.Claims;
using LibrarySystem.Application;
using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Dtos.Responses;
using LibrarySystem.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

POST    /api/auth/register
POST    /api/auth/login
POST    /api/auth/staff/register
POST    /api/auth/staff/login

*/
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IJwtToken _jwt;

    public AuthController(IServiceManager service, IJwtToken jwt)
    {
        _service = service;
        _jwt = jwt;
    }

    [HttpPost("api/auth/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        var user = await _service.UserService.Register(registerUserDto);

        return Created(string.Format("/api/user/{0}", user.Id), null);
    }

    [HttpPost("api/auth/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        var authorized = await _service.UserService.Login(loginUserDto);
        if (!authorized)
        {
            throw new NotAuthorizedException("These credentials are invalid.");
        }

        var user = await _service.UserService.Get(loginUserDto.Email!);
        var token = _jwt.Generate([
            new Claim("AccountId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User"),
        ]);

        return Ok(new LoginUserResponseDto
        {
            UserDto = user.ToDto(),
            Token = token,
        });
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("api/auth/staff/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterStaffDto registerStaffDto)
    {
        var staff = await _service.StaffService.Register(registerStaffDto);

        return Created(string.Format("/api/staff/{0}", staff.Id), null);
    }


    [HttpPost("api/auth/staff/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginStaffDto loginStaffDto)
    {
        var authorized = await _service.StaffService.Login(loginStaffDto);
        if (!authorized)
        {
            throw new NotAuthorizedException("These credentials are invalid.");
        }

        var staff = await _service.StaffService.Get(loginStaffDto.Email!);
        var token = _jwt.Generate([
            new Claim("StaffId", staff.Id.ToString()),
            new Claim(ClaimTypes.Role, "Staff"),
        ]);

        return Ok(new LoginStaffResponseDto
        {
            StaffDto = staff.ToDto(),
            Token = token
        });
    }
}
