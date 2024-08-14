using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Messages;
using LibrarySystem.Domain.Exceptions.Common;
using LibrarySystem.Domain.Exceptions.HTTP;
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
    private readonly IEmailManager _email;
    private readonly IWebHostEnvironment _env;

    public BorrowController(IServiceManager service, IEmailManager email, IWebHostEnvironment env)
    {
        _service = service;
        _email = email;
        _env = env;
    }

    [HttpGet("api/borrow")]
    public async Task<IActionResult> GetBorrows(int limit, int offset, Guid userId, bool active)
    {
        var borrows = await _service.Borrow.GetAll();

        if (userId != Guid.Empty)
            borrows = borrows.Where(b => b.UserId == userId);

        if (active)
            borrows = borrows.Where(b => !b.IsReturned);

        return Ok(borrows.Paginate(offset, limit));
    }

    [HttpGet("api/borrow/{borrowId:guid}")]
    public async Task<IActionResult> GetBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.Get(borrowId);

        return Ok(borrow);
    }

    [HttpPost("api/borrow")]
    public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowDto createBorrowDto)
    {
        var borrow = await _service.Borrow.Create(createBorrowDto);

        // set the book unavailable
        var book = await _service.Book.Get(borrow.BookId);
        _ = _service.Book.SetAvailability(book, false);

        // send confirmation email - FOR PRODUCTION ONLY
        if (_env.IsProduction())
        {
            var user = await _service.User.Get(createBorrowDto.UserId);
            _email.Borrow.SendBorrowBookEmail(new BorrowBookMessageDto
            {
                UserEmail = user.Email!,
                Username = user.Username!,
                BookTitle = book.Title!,
                BookISBN = book.ISBN!,
                BorrowDueDate = borrow.DueDate.ToString("dd-MM-yyyy")
            });
        }
        return Created($"/api/borrow/{borrow.Id}", null);
    }

    [HttpPut("api/borrow/{borrowId:guid}/return")]
    public async Task<IActionResult> ReturnBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.Get(borrowId);
        var book = await _service.Book.Get(borrow.BookId);

        var token = JwtUtils.Parse(HttpContext.Request.Headers.Authorization.FirstOrDefault());

        // match the user ID of the borrow and from the request
        if (borrow.UserId.ToString() != JwtUtils.ParseFromPayload(token, "UserId"))
        {
            throw new NotAuthorized401Exception();
        }

        var affected = await _service.Borrow.Return(borrow, book, token);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        // send confirmation email - FOR PRODUCTION ONLY
        if (_env.IsProduction())
        {
            var user = await _service.User.Get(borrow.UserId);
            _email.Borrow.SendReturnBookEmail(new ReturnBookMessageDto
            {
                UserEmail = user.Email!,
                Username = user.Username!,
                BookTitle = book.Title!,
                BookISBN = book.ISBN!
            });
        }

        return Ok();
    }
}
