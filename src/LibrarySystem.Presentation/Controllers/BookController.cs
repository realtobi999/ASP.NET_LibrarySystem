using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/book params: limit, offset, authorId, genreId
GET     /api/book/{book_id}
GET     /api/book/isbn-{isbn}
POST    /api/book
PUT     /api/book/{book_id}
DELETE  /api/book/{book_id}

*/
public class BookController : ControllerBase
{
    private readonly IServiceManager _service;

    public BookController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("api/book")]
    public async Task<IActionResult> GetBooks(int limit, int offset, Guid authorId, Guid genreId)
    {
        var books = await _service.Book.GetAll();         

        if (authorId != Guid.Empty)
            books = books.Where(b => b.BookAuthors.Any(ba => ba.AuthorId == authorId)); 
        if (genreId != Guid.Empty)
            books = books.Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId));    
        if (offset > 0)
            books = books.Skip(offset).ToList();
        if (limit > 0)
            books = books.Take(limit).ToList(); 

        return Ok(books.Select(b => b.ToDto()));
    }

    [HttpGet("api/book/{bookId:guid}")]
    public async Task<IActionResult> GetBook(Guid bookId)
    {
        var book = await _service.Book.Get(bookId);

        return Ok(book.ToDto());
    }

    [HttpGet("api/book/isbn-{isbn}")]
    public async Task<IActionResult> GetBookByIsbn(string isbn)
    {
        var book = await _service.Book.Get(isbn);

        return Ok(book.ToDto());
    }

    [Authorize(Policy = "Employee")]
    [HttpPost("api/book")]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        var book = await _service.Book.Create(createBookDto);

        return Created(string.Format("/api/book/{0}", book.Id), null);
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/book/{bookId:guid}")]
    public async Task<IActionResult> UpdateBook(Guid bookId, [FromBody] UpdateBookDto updateBookDto, int available)
    {
        var affected = await _service.Book.Update(bookId, updateBookDto);

        if (affected == 0)
            throw new ZeroRowsAffectedException();
            
        if (available == -1)
        {
            await _service.Book.UpdateAvailability(bookId, false);
        }
        if (available == 1)
        {
            await _service.Book.UpdateAvailability(bookId, true);
        }

        return Ok();
    }

    [Authorize(Policy = "Employee")]
    [HttpDelete("api/book/{bookId:guid}")]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        var affected = await _service.Book.Delete(bookId);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    }
}
