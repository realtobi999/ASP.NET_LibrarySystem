using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain.Dtos.Authors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation;

[ApiController]
/*

GET     /api/author param: offset, limit
GET     /api/author/{author_id}
POST    /api/author

*/
public class AuthorController : ControllerBase
{
    private readonly IServiceManager _service;

    public AuthorController(IServiceManager service)
    {
        _service = service;
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
}
