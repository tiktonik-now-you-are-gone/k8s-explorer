using Akka.Actor;
using K8S.Crawler.Host.Actors.Cluster;

namespace K8S.Crawler.Host;

public class ExploreClusterWorker : BackgroundService
{
    private readonly ILogger<ExploreClusterWorker> _logger;

    public ExploreClusterWorker(ILogger<ExploreClusterWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var system = ActorSystem.Create(nameof(ExploreClusterWorker));

        var clusterActor = system.ActorOf<ClustersActor>();

        var response = await clusterActor.Ask<ExploreClusterResponse>(new ExploreClusterRequest
        {
            CorrelationId = Guid.NewGuid().ToString()
        });

        _logger.LogInformation(
            $"Cluster was successfully explored. Founded clusters {string.Join(",", response.Clusters)}");
    }
}