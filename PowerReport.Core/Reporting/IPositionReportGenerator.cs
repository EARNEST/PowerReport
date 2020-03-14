using PowerReport.Core.Entities;

namespace PowerReport.Core.Reporting
{
    public interface IPositionReportGenerator
    {
        string Generate(CalculatedPositionInfo position);
    }
}
