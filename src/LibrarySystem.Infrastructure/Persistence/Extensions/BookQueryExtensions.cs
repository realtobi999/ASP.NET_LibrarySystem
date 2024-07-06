using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Extensions;

public static class BookQueryExtensions
{
    public static IQueryable<Book> IncludeBookRelations(this IQueryable<Book> query)
    {
        query = query.Include(b => b.BookAuthors)
                         .ThenInclude(ba => ba.Author)
                     .Include(b => b.BookGenres)
                         .ThenInclude(bg => bg.Genre)
                     .Include(b => b.BookReviews);

        return query;
    }
}
