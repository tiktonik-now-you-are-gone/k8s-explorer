using K8S.Crawler.Host;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<FinalWorker>(); })
    .Build();

await host.RunAsync();