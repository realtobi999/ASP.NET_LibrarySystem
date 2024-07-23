using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace LibrarySystem.Presentation.Controllers;

[ApiController] 
/*

GET     /api/book params: limit, offset, authorId, genreId, withRelations
GET     /api/book/{book_id} params: withRelations
GET     /api/book/isbn/{isbn} params: withRelations
GET     /api/book/search/{query} params: limit, offset, authorId, genreId, withRelations
POST    /api/book
PUT     /api/book/{book_id}/photos
PUT     /api/book/{book_id}
DELETE  /api/book/{book_id}

*/
public class BookController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRepositoryManager _repository;

    public BookController(IServiceManager service, IRepositoryManager repository)
    {
        _service = service;
        _repository = repository;
    }

    [HttpGet("api/book")]
    public async Task<IActionResult> GetBooks(int limit, int offset, Guid authorId, Guid genreId, bool withRelations = true)
    {
        var books = await _service.Book.GetAll(withRelations);         

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

    [HttpGet("api/book/search/{query}")]
    public async Task<IActionResult> SearchBooks(int limit, int offset, Guid authorId, Guid genreId, string query, bool withRelations = true)
    {
        var books = await _service.Book.GetAll(withRelations);         

        if (authorId != Guid.Empty)
            books = books.Where(b => b.BookAuthors.Any(ba => ba.AuthorId == authorId)); 

        if (genreId != Guid.Empty)
            books = books.Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId));    

        if (!query.IsNullOrEmpty())
            books = books.Where(b => b.Title!.Contains(query) || b.Description!.Contains(query));    

        if (offset > 0)
            books = books.Skip(offset).ToList();

        if (limit > 0)
            books = books.Take(limit).ToList(); 

        return Ok(books.Select(b => b.ToDto()));
    }

    [HttpGet("api/book/{bookId:guid}")]
    public async Task<IActionResult> GetBook(Guid bookId, bool withRelations = true)
    {
        var book = await _service.Book.Get(bookId, withRelations);

        return Ok(book.ToDto());
    }

    [HttpGet("api/book/isbn/{isbn}")]
    public async Task<IActionResult> GetBookByIsbn(string isbn, bool withRelations = true)
    {
        var book = await _service.Book.Get(isbn, withRelations);

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
    public async Task<IActionResult> UpdateBook(Guid bookId, [FromBody] UpdateBookDto updateBookDto)
    {
        var affected = await _service.Book.Update(bookId, updateBookDto);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

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

    [Authorize(Policy = "Employee")]
    [HttpPut("api/book/{bookId:guid}/photos")] 
    public async Task<IActionResult> UploadPhotos(Guid bookId, IFormCollection files)
    {
        var pictures = await _service.Picture.Extract(files);
        var book = await _service.Book.Get(bookId); // validate if book exists

        // delete all previous saved pictures
        _repository.Picture.DeleteWhere(p => p.EntityId == book.Id && p.EntityType == PictureEntityType.Book);

        // assign the id to the pictures and push them to the database
        var affected = await _service.Picture.BulkCreateWithEntity(pictures, book.Id, PictureEntityType.Book); 

        if (affected == 0)
            throw new ZeroRowsAffectedException();
        
        return Ok();
    }
}