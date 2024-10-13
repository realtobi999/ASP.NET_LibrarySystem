using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/author param: offset, limit
GET     /api/author/{author_id}
POST    /api/author
PUT     /api/author/{author_id}/photo
PUT     /api/author/{author_id}
DELETE  /api/author/{author_id}

*/
public class AuthorController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRepositoryManager _repository;
    private readonly IMapperManager _mapper;

    public AuthorController(IServiceManager service, IRepositoryManager repository, IMapperManager mapper)
    {
        _service = service;
        _repository = repository;
        _mapper = mapper;
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("api/author")]
    public async Task<IActionResult> GetAuthors(int limit, int offset)
    {
        var authors = await _service.Author.IndexAsync();

        return Ok(authors.Paginate(offset, limit));
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("api/author/{authorId:guid}")]
    public async Task<IActionResult> GetAuthor(Guid authorId)
    {
        var author = await _service.Author.GetAsync(authorId);

        return Ok(author);
    }

    [Authorize(Policy = "Employee")]
    [HttpPost("api/author")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
    {
        var author = _mapper.Author.Map(createAuthorDto);

        await _service.Author.CreateAsync(author);

        return Created($"/api/author/{author.Id}", null);
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/author/{authorId:guid}")]
    public async Task<IActionResult> UpdateAuthor(Guid authorId, [FromBody] UpdateAuthorDto updateAuthorDto)
    {
        var author = await _service.Author.GetAsync(authorId);

        author.Update(updateAuthorDto);
        await _service.Author.UpdateAsync(author);
        
        return NoContent();
    }

    [Authorize(Policy = "Employee")]
    [HttpDelete("api/author/{authorId:guid}")]
    public async Task<IActionResult> DeleteAuthor(Guid authorId)
    {
        var author = await _service.Author.GetAsync(authorId);

        await _service.Author.DeleteAsync(author);

        return NoContent();
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/author/{authorId:guid}/photo")]
    public async Task<IActionResult> UploadPhoto(Guid authorId, IFormFile file)
    {
        var picture = await _service.Picture.Extract(file);
        var author = await _service.Author.GetAsync(authorId); // validates if the author exists

        // delete any previous associated photo
        _repository.Picture.DeleteWhere(p => p.EntityId == author.Id && p.EntityType == PictureEntityType.Author);

        // assign the id to the pictures and push them to the database
        await _service.Picture.CreateWithEntity(picture, author.Id, PictureEntityType.Author);
        return NoContent();
    }
}
