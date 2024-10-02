using System.Linq.Expressions;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly LibrarySystemContext _context;

    public BaseRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public virtual void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await GetQueryable().FirstOrDefaultAsync(expression);
    }

    public virtual async Task<IEnumerable<T>> IndexAsync()
    {
        return await GetQueryable().ToListAsync();
    }

    protected virtual IQueryable<T> GetQueryable()
    {
        return _context.Set<T>().AsQueryable();
    }
}
