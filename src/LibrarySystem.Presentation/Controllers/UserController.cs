using LibrarySystem.Application.Core.Attributes;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Mappers;
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
    private readonly IUserMapper _mapper;

    public UserController(IServiceManager service, IRepositoryManager repository, IUserMapper mapper)
    {
        _service = service;
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("api/user")]
    public async Task<IActionResult> GetUsers(int limit, int offset)
    {
        var users = await _service.User.IndexAsync();

        return Ok(users.Paginate(offset, limit));
    }

    [HttpGet("api/user/{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _service.User.GetAsync(userId);

        return Ok(user);
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpPut("api/user/{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = await _service.User.GetAsync(userId);

        _mapper.UpdateFromDto(user, updateUserDto);
        await _service.User.UpdateAsync(user);

        return NoContent();
    }

    [Authorize(Policy = "User"), UserAuth]
    [HttpDelete("api/user/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var user = await _service.User.GetAsync(userId);

        await _service.User.DeleteAsync(user);

        return NoContent();
    }

    [Authorize(Policy = "User")]
    [HttpPut("api/user/{userId:guid}/photos")]
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
