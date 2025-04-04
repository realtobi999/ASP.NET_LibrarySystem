﻿using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
[Route("api/genre")]
/*

GET     /api/genre param: limit, offset
GET     /api/genre/{genre_id}
POST    /api/genre
PUT     /api/genre/{genre_id}
DELETE  /api/genre/{genre_id}

*/
public class GenreController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly IMapperManager _mapper;

    public GenreController(IServiceManager service, IMapperManager mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("")]
    public async Task<IActionResult> GetGenres(int limit, int offset)
    {
        var genres = await _service.Genre.IndexAsync();

        return Ok(genres.Paginate(offset, limit));
    }

    [Authorize(Policy = "Employee")]
    [HttpGet("{genreId:guid}")]
    public async Task<IActionResult> GetGenre(Guid genreId)
    {
        var genre = await _service.Genre.GetAsync(genreId);

        return Ok(genre);
    }

    [Authorize(Policy = "Employee")]
    [HttpPost("")]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDto createGenreDto)
    {
        var genre = _mapper.Genre.Map(createGenreDto);

        await _service.Genre.CreateAsync(genre);

        return Created($"/api/genre/{genre.Id}", null);
    }

    [Authorize(Policy = "Employee")]
    [HttpPut("{genreId:guid}")]
    public async Task<IActionResult> UpdateGenre(Guid genreId, [FromBody] UpdateGenreDto updateGenreDto)
    {
        var genre = await _service.Genre.GetAsync(genreId);

        genre.Update(updateGenreDto);
        await _service.Genre.UpdateAsync(genre);

        return NoContent();
    }

    [Authorize(Policy = "Employee")]
    [HttpDelete("{genreId:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid genreId)
    {
        var genre = await _service.Genre.GetAsync(genreId);

        await _service.Genre.DeleteAsync(genre);

        return NoContent();
    }
}