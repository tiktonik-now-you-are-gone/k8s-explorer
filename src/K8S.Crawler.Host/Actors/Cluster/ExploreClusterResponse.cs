using K8S.Crawler.Host.Actors.Core;
using K8S.Crawler.Host.Actors.Deployments;
using K8S.Crawler.Host.Domain;

namespace K8S.Crawler.Host.Actors.Cluster;

public class ExploreClusterResponse : MessageBase
{
    public K8SCluster Cluster { get; set; }
}