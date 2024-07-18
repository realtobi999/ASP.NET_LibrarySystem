﻿using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UpdateUserDto
{
    [Required, MaxLength(55)]
    public string? Username { get; set; }

    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; set; }
}
