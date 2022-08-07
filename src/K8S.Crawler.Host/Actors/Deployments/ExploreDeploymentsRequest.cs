using K8S.Crawler.Host.Actors.Core;

namespace K8S.Crawler.Host.Actors.Deployments;

public class ExploreDeploymentsRequest : MessageBase
{
    public string Namespace { get; set; }
}