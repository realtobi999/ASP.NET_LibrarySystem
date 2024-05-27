using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Entities;

public class Staff
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("Name")]
    public string? Name { get; set; }

    [Required, Column("email")]
    public string? Email { get; set; }

    [Required, Column("password")]
    public string? Password { get; set; }

    public StaffDto ToDto()
    {
        return new StaffDto
        {
            Id = this.Id,
            Name = this.Name,
            Email = this.Email,
        };
    }
}
