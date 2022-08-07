using Akka.Actor;
using k8s;
using K8S.Crawler.Host.Domain;

namespace K8S.Crawler.Host.Actors.Deployments;

public class DeploymentsActor : ReceiveActor
{
    private readonly string _owningNamespace;
    private readonly IServiceProvider _provider;

    public DeploymentsActor(string owningNamespace, IServiceProvider provider)
    {
        _provider = provider;
        _owningNamespace = owningNamespace;

        ReceiveAsync<ExploreDeploymentsRequest>(
            Handle,
            request => request.Namespace == _owningNamespace);
    }

    private async Task Handle(ExploreDeploymentsRequest request)
    {
        using var scope = _provider.CreateScope();

        var k8s = scope.ServiceProvider.GetRequiredService<IKubernetes>();

        var deployments = await k8s.AppsV1.ListNamespacedDeploymentAsync(_owningNamespace);

        Sender.Tell(new ExploreDeploymentsResponse
        {
            CorrelationId = request.CorrelationId,
            Deployments = deployments.Items
                .Select(i => i.Metadata.Name)
                .Select(d => new K8SCluster.K8SNamespace.K8SDeployment
                {
                    Name = d
                }).ToList()
        });
    }

    public static Props Props(string owningNamespace, IServiceProvider serviceProvider)
    {
        return Akka.Actor.Props.Create(() => new DeploymentsActor(owningNamespace, serviceProvider));
    }
}