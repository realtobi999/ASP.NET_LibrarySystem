using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IStaffRepository
{
    Task<IEnumerable<Staff>> GetAll();
    void Create(Staff staff);
    Task<Staff?> Get(string email);
}
