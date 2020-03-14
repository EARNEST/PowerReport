using System;
using System.Text;

namespace PowerReport.Core.Reporting
{
    public class ReportOutcome
    {
        public string Report { get; }
        public DateTime CreatedOnUtc { get; }
        public DateTime Date { get; }
        public string Destination { get; }

        private ReportOutcome(string report, DateTime createdOnUtc, DateTime date, string destination)
        {
            this.Report = report;
            this.CreatedOnUtc = createdOnUtc;
            this.Date = date;
            this.Destination = destination;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Created On UTC: {this.CreatedOnUtc}");
            builder.AppendLine($"Date: {this.Date}");
            builder.AppendLine($"Destination: {this.Destination}");
            builder.AppendLine($"Report: {Environment.NewLine}{this.Report}");

            return builder.ToString();
        }

        public static ReportOutcome Now(string report, DateTime date, string destination) => new ReportOutcome(report, DateTime.UtcNow, date, destination);
    }
}
