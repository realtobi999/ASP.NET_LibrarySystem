using System.Security.Claims;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Email.Messages;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
[Route("api/borrow")]
/*

GET     /api/borrow params: limit, offset, userId, active
GET     /api/borrow/{borrow_id}
POST    /api/borrow
PUT     /api/borrow/{borrow_id}/return

*/
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

    [HttpGet("")]
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

    [HttpGet("{borrowId:guid}")]
    public async Task<IActionResult> GetBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.GetAsync(borrowId);

        return Ok(borrow);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateBorrow([FromBody] CreateBorrowDto createBorrowDto)
    {
        var borrow = _mapper.Borrow.Map(createBorrowDto);
        var book = await _service.Book.GetAsync(borrow.BookId);

        if (!book.IsAvailable)
        {
            throw new Conflict409Exception($"The book with ID: {book.Id} is currently borrowed. Please check the book ID and try again.");
        }

        await _service.Book.UpdateAvailabilityAsync(book, false);
        await _service.Borrow.CreateAsync(borrow);

        // send confirmation email
        if (!_env.IsProduction()) return Created($"/api/borrow/{borrow.Id}", null);

        var user = await _service.User.GetAsync(createBorrowDto.UserId);
        _email.Borrow.SendBorrowBookEmail(new BorrowBookMessageDto
        {
            UserEmail = user.Email,
            Username = user.Username,
            BookTitle = book.Title,
            BookIsbn = book.Isbn,
            BorrowDueDate = borrow.DueDate.ToString("dd-MM-yyyy")
        });
        return Created($"/api/borrow/{borrow.Id}", null);
    }

    [HttpPut("{borrowId:guid}/return")]
    public async Task<IActionResult> ReturnBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.GetAsync(borrowId);
        var book = await _service.Book.GetAsync(borrow.BookId);

        var token = JwtUtils.Parse(HttpContext.Request.Headers.Authorization.FirstOrDefault());
        var role = JwtUtils.ParseFromPayload(token, ClaimTypes.Role);

        if (borrow.IsReturned)
        {
            throw new BadRequest400Exception($"The borrow record for book ID: {book.Id} is already closed. This book has already been IsReturned.");
        }

        if (book.IsAvailable)
        {
            throw new Conflict409Exception($"The book with ID: {book.Id} is not currently borrowed. Please check the book ID and try again.");
        }

        if (DateTimeOffset.UtcNow > borrow.DueDate && role != "Employee") // this role check enables the librarians to return the book even if it's past due
        {
            throw new Conflict409Exception($"The book with ID: {book.Id} cannot be returned because it is past the due date ({borrow.DueDate}). Please contact the library for assistance.");
        }

        await _service.Book.UpdateAvailabilityAsync(book, true);
        await _service.Borrow.UpdateIsReturnedAsync(borrow, true);

        // send confirmation email
        if (!_env.IsProduction()) return NoContent();

        var user = await _service.User.GetAsync(borrow.UserId);
        _email.Borrow.SendReturnBookEmail(new ReturnBookMessageDto
        {
            UserEmail = user.Email,
            Username = user.Username,
            BookTitle = book.Title,
            BookIsbn = book.Isbn
        });

        return NoContent();
    }
}