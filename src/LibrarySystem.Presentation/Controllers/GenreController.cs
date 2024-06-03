﻿using LibrarySystem.Application.Contracts;
using LibrarySystem.Domain.Dtos.Genres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[ApiController]
/*

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
}
