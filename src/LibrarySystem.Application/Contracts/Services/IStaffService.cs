using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IStaffService
{
    Task<Staff> RegisterStaff(RegisterStaffDto registerStaffDto);
}
