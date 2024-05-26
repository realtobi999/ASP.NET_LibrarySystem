using System.Security.Claims;
using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

POST    /api/auth/register
POST    /api/auth/login
POST    /api/auth/staff/register

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
        var user = await _service.UserService.RegisterUser(registerUserDto); 

        return Created(string.Format("/api/user/{0}", user.Id), null);
    }

    [HttpPost("api/auth/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        var authorized = await _service.UserService.LoginUser(loginUserDto);
        if (!authorized)
        {
            throw new NotAuthorizedException("These credentials are invalid.");
        }

        var user = await _service.UserService.GetUserByEmail(loginUserDto.Email!);
        var token = _jwt.Generate([
            new Claim("AccountId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User"),
        ]);

        return Ok(new LoginUserResponseDto{
            User = user,
            Token = token,
        });
    }

    [Authorize(Policy = "Admin")]
    [HttpPost("api/auth/staff/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterStaffDto registerStaffDto)
    {
        var staff = await _service.StaffService.RegisterStaff(registerStaffDto);

        return Created(string.Format("/api/staff/{0}", staff.Id), null);
    }
}
