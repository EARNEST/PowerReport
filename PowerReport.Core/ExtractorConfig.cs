using System;

namespace PowerReport.Core
{
    public class ExtractorConfig
    {
        public TimeSpan MaxTimeout { get; }

        public string LogLocation { get; }

        public string ReportLocation { get; }
        public string Cron { get; }

        public ExtractorConfig(TimeSpan maxTimeout, string logLocation, string reportLocation, string cron)
        {
            this.MaxTimeout = maxTimeout;
            this.LogLocation = logLocation;
            this.ReportLocation = reportLocation;
            this.Cron = cron;
        }
    }
}
