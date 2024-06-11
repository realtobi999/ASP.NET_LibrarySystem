using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/**

POST    /api/book/{book_id}/borrow

**/
public class BorrowController : ControllerBase
{
    private readonly IServiceManager _service;

    public BorrowController(IServiceManager service)
    {
        _service = service;
    }

    [HttpPost("api/borrow")]
    public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowDto createBorrowDto)
    {
        var borrow = await _service.Borrow.Create(createBorrowDto);

        return Created(string.Format("/api/borrow/{0}", borrow.Id), null);
    }
}
