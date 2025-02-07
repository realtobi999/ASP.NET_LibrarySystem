using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Employee : IDtoSerialization<EmployeeDto>
{
    // core properties

    [Required, Column("id")]
    public required Guid Id { get; set; }

    [Required, Column("Name")]
    public required string Name { get; set; }

    [Required, Column("email")]
    public required string Email { get; set; }

    [Required, Column("password")]
    public required string Password { get; set; }

    [Required, Column("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    // auth properties

    [Required, Column("login_attempts")]
    public int LoginAttempts { get; set; }

    [Required, Column("login_lock")]
    public (bool IsLocked, DateTimeOffset DueTo) LoginLock { get; set; }

    // constants

    public static readonly TimeSpan LoginLockDuration = TimeSpan.FromMinutes(15);
    public static readonly int AttemptsBeforeLock = 5;

    // relationships

    [JsonIgnore]
    public Picture? Picture { get; set; }

    /// <inheritdoc/>
    public EmployeeDto ToDto()
    {
        return new EmployeeDto
        {
            Id = this.Id,
            Name = this.Name,
            Email = this.Email,
            Picture = this.Picture
        };
    }

    public void Update(UpdateEmployeeDto dto)
    {
        Email = dto.Email;
        Name = dto.Name;
    }

    public void Lock()
    {
        LoginLock = (IsLocked: true, DueTo: DateTimeOffset.Now.Add(LoginLockDuration));
    }

    public void Unlock()
    {
        LoginLock = default;
    }

    public bool IsLocked()
    {
        return LoginLock.IsLocked && LoginLock.DueTo > DateTimeOffset.Now;
    }
}
