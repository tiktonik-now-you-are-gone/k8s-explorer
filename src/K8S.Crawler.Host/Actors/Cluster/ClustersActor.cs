using Akka.Actor;
using Akka.DependencyInjection;
using K8S.Crawler.Host.Actors.Namespaces;
using K8S.Crawler.Host.Domain;

namespace K8S.Crawler.Host.Actors.Cluster;

public class ClustersActor : ReceiveActor
{
    private IActorRef _namespacesActor;
    
    public ClustersActor()
    {
        ReceiveAsync<ExploreClusterRequest>(Handle);
    }

    private async Task Handle(ExploreClusterRequest request)
    {
        _namespacesActor ??= Context.ActorOf(DependencyResolver.For(Context.System).Props<NamespacesActor>());

        var response = await _namespacesActor.Ask<ExploreNamespacesResponse>(new ExploreNamespacesRequest
        {
            CorrelationId = request.CorrelationId
        });
        
        Sender.Tell(new ExploreClusterResponse
        {
            CorrelationId = request.CorrelationId,
            Cluster = new K8SCluster
            {
                Namespaces = response.Namespaces
            }
        });
    }
}