using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Staffs;

public class StaffService : IStaffService
{
    private readonly IRepositoryManager _repository;
    private readonly IPasswordHasher _hasher;

    public StaffService(IRepositoryManager repository, IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public async Task<Staff> Get(string email)
    {
        var staff = await _repository.Staff.Get(email) ?? throw new NotFoundException($"The staff with email {email} doesnt exist.");

        return staff;
    }

    public async Task<bool> Login(LoginStaffDto loginStaffDto)
    {
        var email = loginStaffDto.Email ?? throw new ArgumentNullException("The email must be set.");
        var password = loginStaffDto.Password ?? throw new ArgumentNullException("The password must be set."); 

        var staff = await _repository.Staff.Get(email) ?? throw new NotFoundException($"The staff with email {email} doesnt exist.");

        return _hasher.Compare(password, staff.Password!);
    }

    public async Task<Staff> Create(RegisterStaffDto registerStaffDto)
    {
        var staff = new Staff
        {
            Id = registerStaffDto.Id ?? Guid.NewGuid(),
            Name = registerStaffDto.Name ?? throw new ArgumentNullException("The name must be set."),
            Email = registerStaffDto.Email ?? throw new ArgumentNullException("The email must be set."),
            Password = _hasher.Hash(registerStaffDto.Password ?? throw new ArgumentNullException("The password must be set.")),
        };

        _repository.Staff.Create(staff);
        await _repository.SaveAsync();

        return staff;
    }

    public Task<IEnumerable<Staff>> GetAll()
    {
        var staff = _repository.Staff.GetAll();

        return staff;
    }
}
