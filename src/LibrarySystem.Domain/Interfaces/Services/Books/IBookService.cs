﻿using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services.Books;

public interface IBookService : IBaseService<Book>
{
    Task<IEnumerable<Book>> IndexRecommendedAsync(User user);
    Task<IEnumerable<Book>> SearchAsync(string query);
    Task<Book> GetAsync(string isbn);
    Task UpdateAvailabilityAsync(Book book, bool isAvailable);
    Task UpdatePopularityAsync(Book book, double popularity);
    double CalculateBookPopularity(Book book);
}