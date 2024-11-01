using LibrarySystem.Domain.Interfaces.Managers;
namespace LibrarySystem.Presentation.Services;

public class BookPopularityBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BookPopularityBackgroundService> _logger;
    private Timer? _timer;

    public BookPopularityBackgroundService(IServiceProvider serviceProvider, ILogger<BookPopularityBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(UpdateBookPopularityScores, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    private async void UpdateBookPopularityScores(object? state)
    {
        _logger.LogInformation("Beginning update of book popularity scores.");

        using var scope = _serviceProvider.CreateScope();
        var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();

        var books = await serviceManager.Book.IndexAsync();
        _logger.LogInformation("Retrieved {Count} books for popularity update.", books.Count());

        foreach (var book in books)
        {
            _logger.LogInformation("Updating book with ID {ID}..", book.Id);

            var popularity = serviceManager.Book.CalculateBookPopularity(book);
            await serviceManager.Book.UpdatePopularityAsync(book, popularity);

            _logger.LogInformation("Updated book with ID {ID} to new popularity score: {Score}.", book.Id, popularity);
        }

        _logger.LogInformation("Completed updating book popularity scores.");
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(stoppingToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}

