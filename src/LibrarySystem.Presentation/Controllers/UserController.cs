using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Exceptions.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/user params: offset, limit
GET     /api/user/{user_id}
PUT     /api/user/{user_id}
PUT     /api/user/{user_id}/photos
DELETE  /api/user/{user_id}

*/
public class UserController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRepositoryManager _repository;

    public UserController(IServiceManager service, IRepositoryManager repository)
    {
        _service = service;
        _repository = repository;
    }

    [HttpGet("api/user")]
    public async Task<IActionResult> GetUsers(int limit, int offset)
    {
        var users = await _service.User.GetAll();

        return Ok(users.Paginate(offset, limit));
    }

    [HttpGet("api/user/{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _service.User.Get(userId);

        return Ok(user);
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPut("api/user/{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        var affected = await _service.User.Update(userId, updateUserDto);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpDelete("api/user/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var affected = await _service.User.Delete(userId);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }

    [Authorize(Policy = "User")]
    [HttpPut("api/user/{userId:guid}/photos")]
    public async Task<IActionResult> UploadPhotos(Guid userId, IFormFile file)
    {
        var picture = await _service.Picture.Extract(file);
        var user = await _service.User.Get(userId); // validate if user exists

        // delete any previous associated photo
        _repository.Picture.DeleteWhere(p => p.EntityId == user.Id  && p.EntityType == PictureEntityType.User);

        // assign the id to the pictures and push them to the database
        var affected = await _service.Picture.CreateWithEntity(picture, user.Id, PictureEntityType.User);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();    
    }
}
