using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation;

[ApiController]
/*

GET     /api/author param: offset, limit
GET     /api/author/{author_id}
POST    /api/author
PUT     /api/author/{author_id}

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
}
