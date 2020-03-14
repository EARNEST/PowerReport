using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace PowerReport.Core.Factories
{
    public class PositionReportExtractorFactory
    {
        public static IPositionReportExtractor Create(ExtractorConfig config) => Create(config, out var logger);

        public static IPositionReportExtractor Create(ExtractorConfig config, out Logger logger)
        {
            var service = PositionServiceProxyFactory.Create(config.MaxTimeout);
            var reporter = PositionReporterFactory.Create(config.ReportLocation);
            logger = CreateLogger(config.LogLocation);

            return new PositionReportExtractor(service, reporter, logger);
        }

        private static Logger CreateLogger(string logLocation)
        {
            var config = new LoggingConfiguration();
            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, new ConsoleTarget("logconsole"));
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, new FileTarget("logfile")
            {
                FileName = Path.Combine(logLocation, "PowerPosition_Logs.txt"),
            });

            LogManager.Configuration = config;

            return LogManager.GetLogger("PITL");
        }
    }
}
