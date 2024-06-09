﻿using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Genres;

namespace LibrarySystem.Domain.Dtos.Books;

public record class BookDto
{
    public Guid Id { get; set; }
    public string? ISBN { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int PagesCount { get; set; } 
    public DateTimeOffset PublishedAt { get; set; }
    public bool Available { get; set; }
    public string? CoverPicture { get; set; }
    public List<AuthorDto> Authors { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
}
