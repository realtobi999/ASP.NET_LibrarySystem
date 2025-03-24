using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibrarySystemContext context) : base(context)
    {
    }

    public Author? Get(Guid id)
    {
        return Context.Authors.FirstOrDefault(a => a.Id == id);
    }

    public async Task<Author?> GetAsync(Guid id)
    {
        return await this.GetAsync(a => a.Id == id);
    }

    protected override IQueryable<Author> GetQueryable()
    {
        return base.GetQueryable().Include(a => a.Picture);
    }
}