using System;
using PowerReport.Core.Reporting;

namespace PowerReport.Core.Factories
{
    public static class PositionReporterFactory
    {
        public static IPositionReporter Create(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new Exception("Invalid empty location for reporter.");
            }

            var generator = new PositionReportGenerator();
            var saver = new PositionReportSaver(location);

            return new PositionReporter(generator, saver);
        }
    }
}
