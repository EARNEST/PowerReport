using System;
using System.Linq;
using System.Text;
using PowerReport.Core.Entities;

namespace PowerReport.Core.Reporting
{
    public class PositionReportGenerator : IPositionReportGenerator
    {
        public string Generate(CalculatedPositionInfo position)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Local Time,Volume");

            if (position.HasPositions)
            {
                var values = position.Positions.Select(p => $"{p.Timestamp},{p.Trade.Volume}");
                var value = string.Join(Environment.NewLine, values);

                builder.Append(value);
            }

            return builder.ToString();
        }
    }
}
