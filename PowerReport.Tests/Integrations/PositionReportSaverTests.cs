using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PowerReport.Core.Entities;
using PowerReport.Core.Reporting;

namespace PowerReport.Tests.Integrations
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    [Category(nameof(PositionReporter))]
    [Category(TestCategory.SlowIntegration)]
    public class PositionReportSaverTests
    {
        private readonly string reportPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "PowerPosition_20150401_1330.csv");
        private readonly DateTime date = DateTime.Parse("01/04/2015 13:30");

        [SetUp]
        [TearDown]
        public void PrePostTestRoutine()
        {
            if (File.Exists(this.reportPath))
            {
                File.Delete(this.reportPath);
            }
        }

        [Test]
        public async Task OnSave_ReportGenerated_Then_ReportSaved()
        {
            // TODO: Use something like SharpFilesystem to avoid testing against actual file system.

            var expectedReport = Guid.NewGuid().ToString();

            var saver = new PositionReportSaver(TestContext.CurrentContext.TestDirectory);

            var mockGenerator = new Mock<IPositionReportGenerator>();
            mockGenerator.Setup(gen => gen.Generate(It.IsAny<CalculatedPositionInfo>()))
                         .Returns(expectedReport);

            var sut = new PositionReporter(mockGenerator.Object, saver);

            var position = CalculatedPositionInfoLocalDate.Create(this.date, Enumerable.Empty<Trade>());

            await sut.ReportAsync(position);

            var actualReport = File.ReadAllText(this.reportPath);

            actualReport.Should().BeEquivalentTo(expectedReport, "Generated report should match.");
            File.Exists(this.reportPath).Should().BeTrue("Report should be saved.");
        }
    }
}
