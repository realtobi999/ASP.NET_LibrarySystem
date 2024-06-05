using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain.Dtos.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/book/{book_id}
POST    /api/book

*/
public class BookController : ControllerBase
{
    private readonly IServiceManager _service;

    public BookController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("api/book/{bookId:guid}")]
    public async Task<IActionResult> GetBook(Guid bookId)
    {
        var book = await _service.Book.Get(bookId);

        return Ok(book.ToDto());
    }

    [Authorize(Policy = "Employee")]
    [HttpPost("api/book")]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        var book = await _service.Book.Create(createBookDto);

        return Created(string.Format("/api/book/{0}", book.Id), null);
    }
}
