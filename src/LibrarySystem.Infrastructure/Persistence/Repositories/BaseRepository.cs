using System.Linq.Expressions;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly LibrarySystemContext Context;

    protected BaseRepository(LibrarySystemContext context)
    {
        Context = context;
    }

    public virtual void Create(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public virtual void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public virtual void Update(T entity)
    {
        Context.Set<T>().Update(entity);
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
        return Context.Set<T>().AsQueryable();
    }
}