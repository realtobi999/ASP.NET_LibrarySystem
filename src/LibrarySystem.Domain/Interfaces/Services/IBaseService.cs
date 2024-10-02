namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBaseService<T>
{
    Task<IEnumerable<T>> IndexAsync();
    Task<T> GetAsync(Guid id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
