using System;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using NLog;
using PowerReport.Core.Reporting;
using PowerReport.Core.Services;

namespace PowerReport.Core
{
    public class PositionReportExtractor : IPositionReportExtractor
    {
        private readonly IPositionService service;
        private readonly IPositionReporter reporter;
        private readonly Logger logger;

        public PositionReportExtractor(IPositionService service, IPositionReporter reporter, Logger logger)
        {
            this.service = service;
            this.reporter = reporter;
            this.logger = logger;
        }

        public Task RunAsync() => this.RunAsync(DateTime.Now);
        public Task RunAsync(DateTime date) => this.RunAsync(date, CancellationToken.None);
        public Task RunAsync(CancellationToken cancellationToken) => this.RunAsync(DateTime.Now, cancellationToken);

        public async Task RunAsync(DateTime date, CancellationToken cancellationToken)
        {
            this.logger.Trace("Extraction started");
            this.logger.Trace("Getting position");
            var resPosition = await this.service.GetPositionAsync(date);
            this.logger.Trace("Position acquired");
            
            cancellationToken.ThrowIfCancellationRequested();

            var result = await resPosition.Match(
                Succ: pos =>
                {
                    this.logger.Trace("Creating report");
                    var outcome = this.reporter.ReportAsync(pos);
                    this.logger.Trace("Report created");

                    return outcome;
                },
                Fail: ex => Task.FromResult(new Result<ReportOutcome>(ex)));

            result.IfSucc(outcome =>
            {
                this.logger.Trace(outcome);
                this.logger.Trace("Extraction successfully finished");
            });

            result.IfFail(ex =>
            {
                this.logger.Trace("Extraction unsuccessfully finished");
                this.logger.Error(ex, "Power position extraction failure.");
            });
        }
    }
}
