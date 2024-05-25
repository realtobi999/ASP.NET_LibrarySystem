using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

POST    /api/auth/register

*/
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;

    public AuthController(IServiceManager service)
    {
        _service = service;
    }

    [HttpPost("api/auth/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        var user = await _service.UserService.RegisterUser(registerUserDto); 

        return Created(string.Format("/api/user/{0}", user.Id), null);
    }
}
