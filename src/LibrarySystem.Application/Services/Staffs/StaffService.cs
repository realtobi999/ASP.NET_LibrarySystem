using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Staffs;

public class StaffService : IStaffService
{
    private readonly IRepositoryManager _repository;

    public StaffService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Staff> RegisterStaff(RegisterStaffDto registerStaffDto)
    {
        var staff = new Staff
        {
            Id = registerStaffDto.Id ?? Guid.NewGuid(),
            Name = registerStaffDto.Name,
            Email = registerStaffDto.Email,
            Password = registerStaffDto.Password,
        };

        _repository.Staff.CreateStaff(staff);
        await _repository.SaveAsync();

        return staff;
    }
}
