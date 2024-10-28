namespace LibrarySystem.Domain.Interfaces.Common;

public interface ISearcher<T>
{
    Task<IEnumerable<T>> SearchAsync(string query);
}
