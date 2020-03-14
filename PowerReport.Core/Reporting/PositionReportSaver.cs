using System;
using System.IO;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;


namespace PowerReport.Core.Reporting
{
    public class PositionReportSaver : IPositionReportSaver
    {
        private readonly string location;

        public PositionReportSaver(string location)
        {
            this.location = location;
        }

        public async Task<Result<ReportOutcome>> SaveAsync(string report, DateTime date) => await TryAsync(this.SaveReportAsync(report, date))();

        private async Task<ReportOutcome> SaveReportAsync(string report, DateTime date)
        {
            this.EnsureLocation();

            var path = this.GetPath(date);

            using (var writer = new StreamWriter(path, false))
            {
                await writer.WriteAsync(report);
            }

            return ReportOutcome.Now(report, date, path);
        }

        private void EnsureLocation()
        {
            if (!Directory.Exists(this.location))
            {
                Directory.CreateDirectory(this.location);
            }

        }

        private string GetPath(DateTime date) => Path.Combine(this.location, $"PowerPosition_{date:yyyyMMdd_HHmm}.csv");
    }
}
