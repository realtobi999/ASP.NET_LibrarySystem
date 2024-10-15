using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Email.Messages;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Mappers;
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
    private readonly IMapperManager _mapper;

    public BorrowController(IServiceManager service, IEmailManager email, IWebHostEnvironment env, IMapperManager mapper)
    {
        _service = service;
        _email = email;
        _env = env;
        _mapper = mapper;
    }

    [HttpGet("api/borrow")]
    public async Task<IActionResult> GetBorrows(int limit, int offset, Guid userId, bool active)
    {
        var borrows = await _service.Borrow.IndexAsync();

        {
            if (userId != Guid.Empty)
                borrows = borrows.Where(b => b.UserId == userId);
        }

        if (active)
        {
            borrows = borrows.Where(b => !b.IsReturned);
        }

        return Ok(borrows.Paginate(offset, limit));
    }

    [HttpGet("api/borrow/{borrowId:guid}")]
    public async Task<IActionResult> GetBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.GetAsync(borrowId);

        return Ok(borrow);
    }

    [HttpPost("api/borrow")]
    public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowDto createBorrowDto)
    {
        var borrow = _mapper.Borrow.Map(createBorrowDto);

        await _service.Borrow.CreateAsync(borrow);

        // set the book unavailable
        var book = await _service.Book.GetAsync(borrow.BookId);
        await _service.Book.SetAvailability(book, false);

        // send confirmation email - FOR PRODUCTION ONLY
        if (_env.IsProduction())
        {
            var user = await _service.User.GetAsync(createBorrowDto.UserId);
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
        var borrow = await _service.Borrow.GetAsync(borrowId);
        var book = await _service.Book.GetAsync(borrow.BookId);

        var token = JwtUtils.Parse(HttpContext.Request.Headers.Authorization.FirstOrDefault());

        // match the user ID of the borrow and from the request
        if (borrow.UserId.ToString() != JwtUtils.ParseFromPayload(token, "UserId"))
        {
            throw new NotAuthorized401Exception();
        }

        await _service.Borrow.ReturnAsync(borrow, book, token, _service.Book.UpdateAsync);

        // send confirmation email - FOR PRODUCTION ONLY
        if (_env.IsProduction())
        {
            var user = await _service.User.GetAsync(borrow.UserId);
            _email.Borrow.SendReturnBookEmail(new ReturnBookMessageDto
            {
                UserEmail = user.Email!,
                Username = user.Username!,
                BookTitle = book.Title!,
                BookISBN = book.ISBN!
            });
        }

        return NoContent();
    }
}
