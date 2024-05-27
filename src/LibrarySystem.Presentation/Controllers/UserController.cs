using LibrarySystem.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/user params: offset, limit

*/
public class UserController : ControllerBase
{
    private readonly IServiceManager _service;

    public UserController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("api/user")]
    public async Task<IActionResult> GetUsers(int limit, int offset)
    {
        var users = await _service.UserService.GetUsers();

        if (offset > 0)
            users = users.Skip(offset);
        if (limit > 0)
            users = users.Take(limit); 

        return Ok(users.Select(u => u.ToDto()));
    }
}
