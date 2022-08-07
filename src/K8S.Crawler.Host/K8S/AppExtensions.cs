using k8s;
using K8S.Crawler.Host.Options;
using Microsoft.Extensions.Options;

namespace K8S.Crawler.Host.K8S;

public static class AppExtensions
{
    public static void AddK8SClient(this IServiceCollection services)
    {
        services.AddSingleton<IKubernetes>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<K8SOptions>>();

            var k8sConfig = new KubernetesClientConfiguration
            {
                Host = options.Value.Api
            };

            return new Kubernetes(k8sConfig);
        });
    }
}