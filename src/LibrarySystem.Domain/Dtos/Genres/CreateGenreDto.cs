﻿using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Genres;

public record class CreateGenreDto
{
    public Guid? Id { get; init; }

    [Required, MaxLength(55)]
    public required string Name { get; init; }
}
