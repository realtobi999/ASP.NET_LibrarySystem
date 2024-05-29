using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IStaffService
{
    Task<IEnumerable<Staff>> GetAll();
    Task<Staff> Create(RegisterStaffDto registerStaffDto);
    Task<bool> Login(LoginStaffDto loginStaffDto);
    Task<Staff> Get(string email);
}
