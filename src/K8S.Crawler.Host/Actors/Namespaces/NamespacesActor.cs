using Akka.Actor;
using Akka.DependencyInjection;
using k8s;
using K8S.Crawler.Host.Actors.Deployments;
using K8S.Crawler.Host.Domain;

namespace K8S.Crawler.Host.Actors.Namespaces;

public class NamespacesActor : ReceiveActor
{
    private readonly IServiceProvider _provider;
    private IDictionary<string, IActorRef> _namespace2Deployment;

    public NamespacesActor(IServiceProvider provider)
    {
        _provider = provider;
        _namespace2Deployment = new Dictionary<string, IActorRef>();
        
        ReceiveAsync<ExploreNamespacesRequest>(Handle);
    }

    private async Task Handle(ExploreNamespacesRequest request)
    {
        using var scope = _provider.CreateScope();

        var k8s = scope.ServiceProvider.GetRequiredService<IKubernetes>();

        var namespacesResponse = await k8s.CoreV1.ListNamespaceAsync();

        var namespaces = namespacesResponse.Items.Select(i => i.Metadata.Name).ToHashSet();

        var exploreTasks = namespaces.Select(ns => ExploreNamespaceDeployments(request.CorrelationId, ns));

        var deployments = await Task.WhenAll(exploreTasks);

        Sender.Tell(new ExploreNamespacesResponse
        {
            CorrelationId = request.CorrelationId,
            Namespaces = deployments.Select(ns2Deployments => new K8SCluster.K8SNamespace
            {
                Name = ns2Deployments.NS,
                Deployments = ns2Deployments.Deployments.Deployments.ToList()
            }).ToList()
        });
    }

    private async Task<(string NS, ExploreDeploymentsResponse Deployments)> ExploreNamespaceDeployments(string correlationId, string ns)
    {
        var deploymentActor = GetOrCreateDeploymentActor(ns);

        var deployments = await deploymentActor.Ask<ExploreDeploymentsResponse>(new ExploreDeploymentsRequest
        {
            CorrelationId = correlationId,
            Namespace = ns
        });

        return (ns, deployments);
    }
    
    private IActorRef GetOrCreateDeploymentActor(string ns)
    {
        if (_namespace2Deployment.ContainsKey(ns))
        {
            return _namespace2Deployment[ns];
        }
        
        _namespace2Deployment[ns] =
            Context.System.ActorOf(DependencyResolver.For(Context.System).Props<DeploymentsActor>(ns));

        return GetOrCreateDeploymentActor(ns);
    }
}
