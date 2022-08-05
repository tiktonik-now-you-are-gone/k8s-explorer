namespace K8S.Crawler.Host;

public class FinalWorker : BackgroundService
{
    private readonly ILogger<FinalWorker> _logger;

    public FinalWorker(ILogger<FinalWorker> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Final worker started at(utc): {time}", DateTimeOffset.UtcNow);
        return Task.CompletedTask;
    }
}