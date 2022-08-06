using Akka.Actor;

namespace K8S.Crawler.Host.Actors.Cluster;

public class ClustersActor : ReceiveActor
{
    public ClustersActor()
    {
        ReceiveAsync<ExploreClusterRequest>(Handle);
    }

    private Task Handle(ExploreClusterRequest request)
    {
        Sender.Tell(new ExploreClusterResponse
        {
            CorrelationId = request.CorrelationId,
            Clusters = new List<string>
            {
                "test-cluster"
            }
        });
        
        return Task.CompletedTask;
    }
}