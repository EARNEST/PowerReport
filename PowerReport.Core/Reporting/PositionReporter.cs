using System.Threading.Tasks;
using LanguageExt;
using PowerReport.Core.Entities;

namespace PowerReport.Core.Reporting
{
    public class PositionReporter : IPositionReporter
    {
        private readonly IPositionReportGenerator generator;
        private readonly IPositionReportSaver saver;

        public PositionReporter(IPositionReportGenerator generator, IPositionReportSaver saver)
        {
            this.generator = generator;
            this.saver = saver;
        }

        public async Task<Result<ReportOutcome>> ReportAsync(CalculatedPositionInfo position)
        {
            var report = this.generator.Generate(position);

            return await this.saver.SaveAsync(report, position.Date);
        }
    }
}
