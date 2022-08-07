using K8S.Crawler.Host.Actors.Core;
using K8S.Crawler.Host.Domain;

namespace K8S.Crawler.Host.Actors.Deployments;

public class ExploreDeploymentsResponse : MessageBase
{
    public ICollection<K8SCluster.K8SNamespace.K8SDeployment> Deployments { get; set; }
}
