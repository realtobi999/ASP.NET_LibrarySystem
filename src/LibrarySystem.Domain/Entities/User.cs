using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("username")]
    public string? Username { get; set; }

    [Required, Column("email")]
    public string? Email { get; set; }

    [Required, Column("password")]
    public string? Password { get; set; }
}