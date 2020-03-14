using System;
using System.Threading;
using System.Threading.Tasks;

namespace PowerReport.Core
{
    public interface IPositionReportExtractor
    {
        Task RunAsync();
        Task RunAsync(DateTime date);

        Task RunAsync(CancellationToken cancellationToken);
        Task RunAsync(DateTime date, CancellationToken cancellationToken);
    }
}
