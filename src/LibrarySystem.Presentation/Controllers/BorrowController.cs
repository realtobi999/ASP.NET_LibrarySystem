using LibrarySystem.Application.Contracts;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/**

GET     /api/borrow params: limit, offset, userId, active
GET     /api/borrow/{borrow_id}
POST    /api/borrow
PUT     /api/borrow/{borrow_id}/return

**/
public class BorrowController : ControllerBase
{
    private readonly IServiceManager _service;

    public BorrowController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("api/borrow")]
    public async Task<IActionResult> GetBorrows(int limit, int offset, Guid userId, bool active)
    {
        var borrows = await _service.Borrow.GetAll();

        if (userId != Guid.Empty)
            borrows = borrows.Where(b => b.UserId == userId);
            
        if (active)
            borrows = borrows.Where(b => !b.Returned);

        if (offset > 0)
            borrows = borrows.Skip(offset);
            
        if (limit > 0)
            borrows = borrows.Take(limit); 

        return Ok(borrows.Select(b => b.ToDto()));
    }

    [HttpGet("api/borrow/{borrowId:guid}")]
    public async Task<IActionResult> GetBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.Get(borrowId);

        return Ok(borrow.ToDto());
    }

    [HttpPost("api/borrow")]
    public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowDto createBorrowDto)
    {
        var borrow = await _service.Borrow.Create(createBorrowDto);

        return Created(string.Format("/api/borrow/{0}", borrow.Id), null);
    }

    [HttpPut("api/borrow/{borrowId:guid}/return")]
    public async Task<IActionResult> ReturnBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.Get(borrowId);

        // extract the jwt token to get the user id from the request
        var token = Jwt.Parse(HttpContext.Request.Headers.Authorization.FirstOrDefault());
        var payload = Jwt.ParsePayload(token);
        var tokenUserId = payload.FirstOrDefault(c => c.Type.Equals("USERID", StringComparison.CurrentCultureIgnoreCase))?.Value;

        // match the user id of the borrow and from the request
        if (borrow.UserId.ToString() != tokenUserId)
        {
            throw new NotAuthorizedException("You are not authorized to return this book.");
        }

        var affected = await _service.Borrow.Return(borrow);
        
        if (affected == 0)
            throw new ZeroRowsAffectedException();
         
        return Ok();
    }
}
