﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Genre : IDtoSerialization<GenreDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("name")]
    public string? Name { get; set; }

    /// <inheritdoc/>
    public GenreDto ToDto()
    {
        return new GenreDto
        {
            Id = this.Id,
            Name = this.Name
        };
    }

    public void Update(UpdateGenreDto dto)
    {
        Name = dto.Name;
    }
}
