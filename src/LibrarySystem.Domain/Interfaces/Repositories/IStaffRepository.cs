using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IStaffRepository
{
    void CreateStaff(Staff staff);
}
