using Akka.Actor;
using Akka.Actor.Setup;
using Akka.DependencyInjection;
using K8S.Crawler.Host.Actors.Cluster;
using K8S.Crawler.Host.Domain;

namespace K8S.Crawler.Host;

public class ExploreClusterWorker : BackgroundService
{
    private readonly ILogger<ExploreClusterWorker> _logger;
    private readonly IServiceProvider _provider;

    public ExploreClusterWorker(ILogger<ExploreClusterWorker> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var diSetup = DependencyResolverSetup.Create(_provider);
        
        var system = ActorSystem.Create(nameof(ExploreClusterWorker), ActorSystemSetup.Create(diSetup));

        var clusterActor = system.ActorOf<ClustersActor>();

        var response = await clusterActor.Ask<ExploreClusterResponse>(new ExploreClusterRequest
        {
            CorrelationId = Guid.NewGuid().ToString()
        });

        PrintWithAlignment("Cluster was successfully explored.", 0);
        
        PrintNamespaces(response.Cluster.Namespaces);
    }

    private void PrintNamespaces(ICollection<K8SCluster.K8SNamespace> namespaces)
    {
        PrintWithAlignment($"namespaces", 1);
        foreach (var ns in namespaces)
        {
            PrintWithAlignment(ns.Name, 2);
            PrintDeployment(ns.Deployments);
        }
    }

    private void PrintDeployment(ICollection<K8SCluster.K8SNamespace.K8SDeployment> deployments)
    {
        PrintWithAlignment($"deployments", 3);
        foreach (var deployment in deployments)
        {
            PrintWithAlignment(deployment.Name, 4);
        }
    }
    
    private void PrintWithAlignment(string message, int level)
    {
        const string alignmentUnit = " ";
        const int alignmentMinUnits = 4;

        string alignment = string.Join(
            string.Empty,
            Enumerable.Range(0, alignmentMinUnits * level).Select(_ => alignmentUnit));
        
        Console.WriteLine($"{alignment}{message}");
    }
}