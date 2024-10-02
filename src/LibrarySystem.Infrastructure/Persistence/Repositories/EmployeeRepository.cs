using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(LibrarySystemContext context) : base(context)
    {
    }

    public async Task<Employee?> GetAsync(string email)
    {
        return await this.GetAsync(e => e.Email == email);
    }

    public async Task<Employee?> GetAsync(Guid id)
    {
        return await this.GetAsync(e => e.Id == id);
    }
}
