using LibrarySystem.Application;
using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/user params: offset, limit
GET     /api/user/{user_id}
PUT     /api/user/{user_id}
DELETE  /api/user/{user_id}

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
        var users = await _service.User.GetAll();

        if (offset > 0)
            users = users.Skip(offset);
        if (limit > 0)
            users = users.Take(limit); 

        return Ok(users.Select(u => u.ToDto()));
    }

    [HttpGet("api/user/{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _service.User.Get(userId);

        return Ok(user.ToDto());
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPut("api/user/{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        var affected = await _service.User.Update(userId, updateUserDto);

        if (affected == 0)
        {
            throw new InternalServerErrorException("Zero affected rows while trying to modify the database.");
        }

        return Ok();
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpDelete("api/user/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var affected = await _service.User.Delete(userId);

        if (affected == 0)
        {
            throw new InternalServerErrorException("Zero affected rows while trying to modify the database!");
        }

        return Ok();
    }
}
