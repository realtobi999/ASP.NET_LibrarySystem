﻿using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
[Route("api/user")]
/*

GET     /api/user params: offset, limit
GET     /api/user/{user_id}
PUT     /api/user/{user_id}
PUT     /api/user/{user_id}/photo
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

    [HttpGet("")]
    public async Task<IActionResult> GetUsers(int limit, int offset)
    {
        var users = await _service.User.IndexAsync();

        return Ok(users.Paginate(offset, limit));
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _service.User.GetAsync(userId);

        return Ok(user);
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = await _service.User.GetAsync(userId);

        user.Update(updateUserDto);
        await _service.User.UpdateAsync(user);

        return NoContent();
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var user = await _service.User.GetAsync(userId);

        await _service.User.DeleteAsync(user);

        return NoContent();
    }

    [Authorize(Policy = "User")]
    [HttpPut("{userId:guid}/photo")]
    public async Task<IActionResult> UploadPhotos(Guid userId, IFormFile file)
    {
        var picture = await _service.Picture.Extract(file);
        var user = await _service.User.GetAsync(userId); // validate if user exists

        // delete any previous associated photo
        _repository.Picture.DeleteWhere(p => p.EntityId == user.Id && p.EntityType == PictureEntityType.User);

        // assign the id to the pictures and push them to the database
        await _service.Picture.CreateWithEntity(picture, user.Id, PictureEntityType.User);

        return NoContent();
    }
}