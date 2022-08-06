namespace K8S.Crawler.Host.Actors.Cluster;

public class ExploreClusterResponse
{
    // todo: introduce base class
    public string CorrelationId { get; set; }

    public IList<string> Clusters { get; set; }
}