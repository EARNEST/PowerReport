using Topshelf;

namespace PowerReport.Runner
{
    internal static class ServiceHosting
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<ExtractorJobService>(service =>
                {
                    service.ConstructUsing(settings => new ExtractorJobService());
                    service.WhenStarted(async s => await s.StartAsync());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();

                configure.SetServiceName("PowerPositionExtractor");
                configure.SetDisplayName("Power Position Extractor");
                configure.SetDescription("Power position extractor");
            });
        }
    }
}
