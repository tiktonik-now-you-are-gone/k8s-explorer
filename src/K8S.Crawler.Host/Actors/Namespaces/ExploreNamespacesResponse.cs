using K8S.Crawler.Host.Actors.Core;
using K8S.Crawler.Host.Domain;

namespace K8S.Crawler.Host.Actors.Namespaces;

public class ExploreNamespacesResponse : MessageBase
{
    public ICollection<K8SCluster.K8SNamespace> Namespaces { get; set; }
}