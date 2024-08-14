using LibrarySystem.Domain.Exceptions.HTTP;

namespace LibrarySystem.Application.Core.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int offset, int limit)
    {
        if (offset == 0 || limit == 0)
        {
            return source;
        }

        if (offset < 0) throw new BadRequest400Exception("Offset must be greater than zero.");
        if (limit < 0) throw new BadRequest400Exception("Limit must be greater than zero.");

        source = source.Skip(offset);
        source = source.Take(limit);

        return source;
    }
}
