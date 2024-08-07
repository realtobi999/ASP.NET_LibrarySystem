﻿using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Authors;

public record class AuthorDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public Picture? Picture { get; set; }
}
