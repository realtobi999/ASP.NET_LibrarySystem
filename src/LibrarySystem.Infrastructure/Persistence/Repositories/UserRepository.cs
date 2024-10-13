using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LibrarySystemContext context) : base(context)
    {
    }

    public async Task<User?> GetAsync(Guid id)
    {
        return await this.GetAsync(u => u.Id == id);
    }

    public async Task<User?> GetAsync(string email)
    {
        return await this.GetAsync(u => u.Email == email);
    }

    protected override IQueryable<User> GetQueryable()
    {
        return base.GetQueryable()
                   .Include(u => u.ProfilePicture)
                   .Include(u => u.BookReviews)
                   .Include(u => u.Wishlists)
                   .Include(u => u.Borrows);
    }
}
