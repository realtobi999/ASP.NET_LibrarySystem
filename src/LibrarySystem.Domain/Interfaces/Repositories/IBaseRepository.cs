using System.Linq.Expressions;

namespace LibrarySystem.Domain.Interfaces.Repositories;

/// <summary>
/// Defines the basic operations for a repository handling entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of entity the repository manages.</typeparam>
public interface IBaseRepository<T>
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an collection of <typeparamref name="T"/> entities.</returns>
    Task<IEnumerable<T>> IndexAsync();

    /// <summary>
    /// Retrieves a single entity based on a specified condition.
    /// </summary>
    /// <param name="expression">An expression that defines the condition for selecting the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, <c>null</c>.</returns>
    Task<T?> GetAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    void Create(T entity);

    /// <summary>
    /// Removes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);
}