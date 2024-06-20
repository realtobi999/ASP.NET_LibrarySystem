using System.Security.Claims;
using LibrarySystem.Application.Contracts;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
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
    private readonly IEmailSender _email;
    private readonly IMessageBuilder _messageBuilder;

    public BorrowController(IServiceManager service, IEmailSender email, IMessageBuilder messageBuilder)
    {
        _service = service;
        _email = email;
        _messageBuilder = messageBuilder;
    }

    [HttpGet("api/borrow")]
    public async Task<IActionResult> GetBorrows(int limit, int offset, Guid userId, bool active)
    {
        var borrows = await _service.Borrow.GetAll();

        if (userId != Guid.Empty)
            borrows = borrows.Where(b => b.UserId == userId);

        if (active)
            borrows = borrows.Where(b => !b.IsReturned);

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

        // set the book unavailable
        var book = await _service.Book.Get(borrow.BookId);
        _ = _service.Book.SetAvailable(book, false);

        return Created(string.Format("/api/borrow/{0}", borrow.Id), null);
    }

    [HttpPut("api/borrow/{borrowId:guid}/return")]
    public async Task<IActionResult> ReturnBorrow(Guid borrowId)
    {
        var borrow = await _service.Borrow.Get(borrowId);
        var book = await _service.Book.Get(borrow.BookId);

        // extract the content of the JWT token payload
        var jwtToken = Jwt.Parse(HttpContext.Request.Headers.Authorization.FirstOrDefault());
        var userIdFromToken = Jwt.ParseFromPayload(jwtToken, "UserId");
        var userRole = Jwt.ParseFromPayload(jwtToken, ClaimTypes.Role);

        // match the user ID of the borrow and from the request
        if (borrow.UserId.ToString() != userIdFromToken)
        {
            throw new NotAuthorizedException("You are not authorized to return this book.");
        }

        if (borrow.IsReturned)
        {
            throw new BadRequestException($"The borrow record for book ID: {book.Id} is already closed. This book has already been IsReturned.");
        }
        if (book.IsAvailable) 
        {
            throw new ConflictException($"The book with ID: {book.Id} is not currently borrowed. Please check the book ID and try again.");
        }
        if (DateTimeOffset.UtcNow > borrow.DueDate && userRole != "Employee") // this role check enables the librarians to return the book even if it's past due
        {
            throw new ConflictException($"The book with ID: {book.Id} cannot be returned because it is past the due date ({borrow.DueDate}). Please contact the library for assistance.");
        }

        _ = _service.Borrow.SetIsReturned(borrow);
        _ = _service.Book.SetAvailable(book);

        var user = await _service.User.Get(borrow.UserId);
        var message = _messageBuilder.BuildBookReturnMessage(user.Email!, new ReturnBookMessageDto{
            Username = user.Username!,
            BookTitle = book.Title!,
            BookISBN = book.ISBN!
        });

        _email.SendEmail(message);

        return Ok();
    }
}
