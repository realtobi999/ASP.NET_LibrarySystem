using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/book params: limit, offset, authorId, genreId, 
GET     /api/book/recent params: limit, offset, authorId, genreId, 
GET     /api/book/popular params: limit, offset, authorId, genreId, 
GET     /api/book/recommended params: limit, offset 
GET     /api/book/search/{query} params: limit, offset, authorId, genreId, 
GET     /api/book/{book_id}  
GET     /api/book/isbn/{isbn} 
POST    /api/book
PUT     /api/book/{book_id}/photo
PUT     /api/book/{book_id}
DELETE  /api/book/{book_id}

*/
public class BookController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IRepositoryManager _repository;
    private readonly IMapperManager _mapper;

    public BookController(IServiceManager service, IRepositoryManager repository, IMapperManager mapper)
    {
        _service = service;
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("api/book")]
    [HttpGet("api/book/recent")]
    [HttpGet("api/book/popular")]
    public async Task<IActionResult> GetBooks(string? query, int limit, int offset, Guid authorId, Guid genreId)
    {
        var books = await _service.Book.IndexAsync();

        // if the url is api/book/recent order the books by the CreatedAt property
        if (HttpContext.Request.GetDisplayUrl().Contains("/api/book/recent"))
        {
            books = books.OrderByDescending(b => b.CreatedAt);
        }

        // if the url is api/book/popular order the books by the Popularity property
        if (HttpContext.Request.GetDisplayUrl().Contains("/api/book/popular"))
        {
            books = books.OrderBy(b => b.Popularity);
        }

        if (authorId != Guid.Empty)
        {
            books = books.Where(b => b.Authors.Any(a => a.Id == authorId));
        }

        if (genreId != Guid.Empty)
        {
            books = books.Where(b => b.Genres.Any(g => g.Id == genreId));
        }

        return Ok(books.Paginate(offset, limit));
    }

    [HttpGet("api/book/search/{query}")]
    public async Task<IActionResult> SearchBooks(string query, int limit, int offset)
    {
        var books = await _service.Book.SearchAsync(query);

        return Ok(books.Paginate(offset, limit));
    }

    [Authorize(Policy = "User")]
    [HttpGet("api/book/recommended")]
    public async Task<IActionResult> GetRecommendedBooks(int limit, int offset)
    {
        var token = JwtUtils.Parse(HttpContext.Request.Headers.Authorization.FirstOrDefault());

        var user = await _service.User.GetAsync(JwtUtils.ParseFromPayload(token, "UserId") ?? throw new NotAuthorized401Exception());
        var books = await _service.Book.IndexRecommendedAsync(user);

        return Ok(books.Paginate(offset, limit).OrderBy(b => b.Popularity));
    }

    [HttpGet("api/book/{bookId:guid}")]
    public async Task<IActionResult> GetBook(Guid bookId)
    {
        var book = await _service.Book.GetAsync(bookId);

        return Ok(book);
    }

    [HttpGet("api/book/isbn/{isbn}")]
    public async Task<IActionResult> GetBookByIsbn(string isbn)
    {
        var book = await _service.Book.GetAsync(isbn);

        return Ok(book);
    }

    [Authorize(Policy = "Employee")]
    [HttpPost("api/book")]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        var book = _mapper.Book.Map(createBookDto);

        await _service.Book.CreateAsync(book);

        return Created($"/api/book/{book.Id}", null);
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/book/{bookId:guid}")]
    public async Task<IActionResult> UpdateBook(Guid bookId, [FromBody] UpdateBookDto updateBookDto)
    {
        var book = await _service.Book.GetAsync(bookId);

        var genres = new List<Genre>();
        var authors = new List<Author>();

        foreach (var authorId in updateBookDto.AuthorIds)
        {
            var author = await _service.Author.GetAsync(authorId);
            authors.Add(author);
        }
        foreach (var genreId in updateBookDto.GenreIds)
        {
            var genre = await _service.Genre.GetAsync(genreId);
            genres.Add(genre);
        }

        book.Update(updateBookDto, genres, authors);
        await _service.Book.UpdateAsync(book);

        return NoContent();
    }

    [Authorize(Policy = "Employee")]
    [HttpDelete("api/book/{bookId:guid}")]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        var book = await _service.Book.GetAsync(bookId);

        await _service.Book.DeleteAsync(book);

        return NoContent();
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/book/{bookId:guid}/photo")]
    public async Task<IActionResult> UploadPhotos(Guid bookId, IFormCollection files)
    {
        var pictures = await _service.Picture.Extract(files);
        var book = await _service.Book.GetAsync(bookId); // validate if book exists

        // delete all previous saved pictures
        _repository.Picture.DeleteWhere(p => p.EntityId == book.Id && p.EntityType == PictureEntityType.Book);

        // assign the id to the pictures and push them to the database
        await _service.Picture.BulkCreateWithEntity(pictures, book.Id, PictureEntityType.Book);

        return NoContent();
    }
}