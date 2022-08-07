using K8S.Crawler.Host;
using K8S.Crawler.Host.K8S;
using K8S.Crawler.Host.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<K8SOptions>(context.Configuration.GetSection(K8SOptions.ConfigSectionName));

        services.AddK8SClient();

        services.AddHostedService<ExploreClusterWorker>();
    })
    .Build();

await host.RunAsync();