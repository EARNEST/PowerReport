using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PowerReport.Core.Entities;
using PowerReport.Core.Reporting;

namespace PowerReport.Tests.Units
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    [Category(nameof(PositionReportGenerator))]
    public class PositionReportGeneratorTests
    {
        [Test]
        public void Generate_Only_Headers_Without_Trades()
        {
            var sut = new PositionReportGenerator();

            var position = CalculatedPositionInfoLocalDate.Create(DateTime.UtcNow, Enumerable.Empty<Trade>());

            var report = sut.Generate(position);

            var truncatedReport = report.Replace(Environment.NewLine, string.Empty);

            truncatedReport.Should().Be("Local Time,Volume", "Strict report check. Headers must match.");
        }

        [Test]
        public void Generate_Rows()
        {
            var sut = new PositionReportGenerator();

            var position = CalculatedPositionInfoLocalDate.Create(
                date: DateTime.Parse("01/04/2015"),
                trades: new[]
                {
                    Trade.Create(19, -42)
                });

            var report = sut.Generate(position);

            var indexHeaderEnd = report.IndexOf(Environment.NewLine, StringComparison.Ordinal);

            var truncatedReport = report
                                 .Substring(indexHeaderEnd)
                                 .Replace(Environment.NewLine, string.Empty);

            truncatedReport.Should().Be("17:00,-42", "Strict report check. Rows must match.");
        }
    }
}
