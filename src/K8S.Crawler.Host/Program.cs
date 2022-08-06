using K8S.Crawler.Host;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ExploreClusterWorker>();
    })
    .Build();

await host.RunAsync();