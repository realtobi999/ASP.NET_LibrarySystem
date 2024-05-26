using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IStaffRepository
{
    void CreateStaff(Staff staff);
    Task<Staff?> GetStaffByEmail(string email);
}
