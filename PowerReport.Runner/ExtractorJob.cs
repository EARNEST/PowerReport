using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NCrontab;
using NLog;
using PowerReport.Core;
using PowerReport.Core.Factories;

namespace PowerReport.Runner
{
    public class ExtractorJob
    {
        private readonly IPositionReportExtractor extractor;
        private readonly Logger logger;
        private readonly ExtractorConfig config;

        private CancellationTokenSource tokenSource;

        public string JobId { get; }
        public string Cron => this.config.Cron;

        public ExtractorJob()
        {
            this.JobId = nameof(ExtractorJob);

            this.config = new ExtractorConfig(
                maxTimeout: TimeSpan.Parse(ConfigurationManager.AppSettings["MaxTimeout"]),
                logLocation: ConfigurationManager.AppSettings["LogLocation"],
                reportLocation: ConfigurationManager.AppSettings["ReportLocation"],
                cron: ConfigurationManager.AppSettings["Cron"]);

            this.extractor = PositionReportExtractorFactory.Create(this.config, out var logger);
            this.logger = logger;
        }

        public async Task StartAsync()
        {
            this.logger.Info("Service started.");

            this.LogNextTopOccurrencesAt();

            this.tokenSource = new CancellationTokenSource();

            await this.extractor.RunAsync(this.tokenSource.Token);
        }

        public void Stop()
        {
            this.logger.Info("Service stopped.");

            this.tokenSource.Cancel();
        }

        private void LogNextTopOccurrencesAt()
        {
            var nextTopCount = 5;
            var nextTopOccurrencesAt = CrontabSchedule
                                      .Parse(this.config.Cron)
                                      .GetNextOccurrences(DateTime.Now, DateTime.Now.AddMinutes(30))
                                      .Take(nextTopCount)
                                      .ToArray();

            this.logger.Info($"Top {nextTopCount} next occurrences based on provided cron: {string.Join(", ", nextTopOccurrencesAt)}");
        }
    }
}
