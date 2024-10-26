using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
namespace LibrarySystem.Presentation.Services;

public class BookPopularityBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBookPopularityCalculator _popularityCalculator;
    private Timer? _timer;

    public BookPopularityBackgroundService(IServiceProvider serviceProvider, IBookPopularityCalculator popularityCalculator)
    {
        _serviceProvider = serviceProvider;
        _popularityCalculator = popularityCalculator;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(UpdateBookPopularityScores, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        return Task.CompletedTask;
    }

    private async void UpdateBookPopularityScores(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();

        var books = await serviceManager.Book.IndexAsync();

        foreach (var book in books)
        {
            await serviceManager.Book.UpdatePopularityAsync(book, _popularityCalculator.CalculatePopularityScore(book));
        }
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

