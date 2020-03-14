using System;
using System.Threading.Tasks;
using LanguageExt;

namespace PowerReport.Core.Reporting
{
    public interface IPositionReportSaver
    {
        Task<Result<ReportOutcome>> SaveAsync(string report, DateTime date);
    }
}
