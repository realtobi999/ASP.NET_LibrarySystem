using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation;

[ApiController]
/*

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
    [HttpPost("api/author")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
    {
        var author = await _service.Author.Create(createAuthorDto);

        return Created(string.Format("/api/author/{0}", author.Id), null);
    }
}
