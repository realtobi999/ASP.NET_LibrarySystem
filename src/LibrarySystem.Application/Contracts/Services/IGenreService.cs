﻿using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Contracts.Services;

public interface IGenreService
{
    Task<Genre> Create(CreateGenreDto createGenreDto);
}
