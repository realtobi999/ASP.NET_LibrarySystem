using System;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Common;

public interface IBookRecommender
{
    Task<IEnumerable<Book>> IndexRecommendedAsync(User user);
}
