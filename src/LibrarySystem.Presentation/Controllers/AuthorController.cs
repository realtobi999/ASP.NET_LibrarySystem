using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/author param: offset, limit
GET     /api/author/{author_id}
POST    /api/author
POST    /api/author/{author_id}/photos/upload
PUT     /api/author/{author_id}
DELETE  /api/author/{author_id}

*/
public class AuthorController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRepositoryManager _repository;

    public AuthorController(IServiceManager service, IRepositoryManager repository)
    {
        _service = service;
        _repository = repository;
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("api/author")]
    public async Task<IActionResult> GetAuthors(int limit, int offset)
    {
        var authors = await _service.Author.GetAll();

        if (offset > 0)
            authors = authors.Skip(offset);
        if (limit > 0)
            authors = authors.Take(limit);

        return Ok(authors.Select(a => a.ToDto()));
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("api/author/{authorId:guid}")]
    public async Task<IActionResult> GetAuthor(Guid authorId)
    {
        var author = await _service.Author.Get(authorId);

        return Ok(author.ToDto());
    }

    [Authorize(Policy = "Employee")]
    [HttpPost("api/author")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
    {
        var author = await _service.Author.Create(createAuthorDto);

        return Created(string.Format("/api/author/{0}", author.Id), null);
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/author/{authorId:guid}")]
    public async Task<IActionResult> UpdateAuthor(Guid authorId, [FromBody] UpdateAuthorDto updateAuthorDto)
    {
        var affected = await _service.Author.Update(authorId, updateAuthorDto);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }

    [Authorize(Policy = "Employee")]
    [HttpDelete("api/author/{authorId:guid}")]
    public async Task<IActionResult> DeleteAuthor(Guid authorId)
    {
        var affected = await _service.Author.Delete(authorId);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }

    [Authorize(Policy = "Employee")]
    [HttpPatch("api/author/{authorId:guid}/photos/upload")]
    public async Task<IActionResult> UploadPhotos(Guid authorId, IFormFile file)
    {
        var picture = await _service.Picture.Extract(file);
        var author = await _service.Author.Get(authorId); // validates if the author exists

        // delete any previous associated photo
        _repository.Picture.DeleteWhere(p => p.EntityId == author.Id  && p.EntityType == PictureEntityType.Author);

        // assign the id to the pictures and push them to the database
        var affected = await _service.Picture.CreateWithEntity(picture, author.Id, PictureEntityType.Author);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();    
    }
}
