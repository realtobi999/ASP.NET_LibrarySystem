using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

GET     /api/genre param: limit, offset
GET     /api/genre/{genre_id}
POST    /api/genre

*/
public class GenreController : ControllerBase
{
    private readonly IServiceManager _service;

    public GenreController(IServiceManager service)
    {
        _service = service;
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("api/genre")]
    public async Task<IActionResult> GetGenres(int limit, int offset)
    {
        var genres = await _service.Genre.GetAll();
        
        if (offset > 0)
            genres = genres.Skip(offset);
        if (limit > 0)
            genres = genres.Take(limit);

        return Ok(genres.Select(g => g.ToDto()));
    }    

    [Authorize(Policy = "Employee")]
    [HttpGet("api/genre/{genreId:guid}")]
    public async Task<IActionResult> GetGenre(Guid genreId)
    {
        var genre = await _service.Genre.Get(genreId);

        return Ok(genre.ToDto());
    }

    [Authorize(Policy = "Employee")]
    [HttpPost("api/genre")]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDto createGenreDto)
    {
        var genre = await _service.Genre.Create(createGenreDto);

        return Created(string.Format("/api/genre/{0}", genre.Id), null);
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("api/genre/{genreId:guid}")]
    public async Task<IActionResult> UpdateGenre(Guid genreId, [FromBody] UpdateGenreDto updateGenreDto)
    {
        var affected = await _service.Genre.Update(genreId, updateGenreDto);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    } 

    [Authorize(Policy = "Employee")]
    [HttpDelete("api/genre/{genreId:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid genreId)
    {
        var affected = await _service.Genre.Delete(genreId);

        if (affected == 0)
            throw new ZeroRowsAffectedException();

        return Ok();
    } 
}
