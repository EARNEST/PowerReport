using System.Threading.Tasks;
using LanguageExt;
using PowerReport.Core.Entities;

namespace PowerReport.Core.Reporting
{
    public interface IPositionReporter
    {
        Task<Result<ReportOutcome>> ReportAsync(CalculatedPositionInfo position);
    }
}
