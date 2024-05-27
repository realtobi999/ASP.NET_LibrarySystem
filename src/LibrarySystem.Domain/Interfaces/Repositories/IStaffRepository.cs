using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IStaffRepository
{
    void Create(Staff staff);
    Task<Staff?> Get(string email);
}
