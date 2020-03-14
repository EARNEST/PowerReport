using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PowerReport.Core.Services;
using PowerReport.Core.Services.Adapter;

namespace PowerReport.Tests.Units
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    [Category(nameof(PositionService))]
    public class PositionServiceTests
    {
        [Test]
        public async Task GetPosition_On_Different_Trade_Dates_Invalid()
        {
            var mockAdapter = new Mock<IPowerServiceAdapter>();
            mockAdapter.Setup(adapter => adapter.GetTradesAsync(It.IsAny<DateTime>()))
                       .ReturnsAsync(new[]
                        {
                            new PowerTradeDto
                            {
                                Date = DateTime.Parse("2015/01/01"),
                            },
                            new PowerTradeDto
                            {
                                Date = DateTime.Parse("2015/01/02"),
                            },
                        });

            var sut = new PositionService(mockAdapter.Object);


            var result = await sut.GetPositionAsync(It.IsAny<DateTime>());

            result.IsFaulted.Should().BeTrue();
        }

        [Test]
        public async Task GetPosition_With_Correct_Mapping()
        {
            var date = DateTime.Parse("2015/01/01");
            var period = new PowerPeriodDto
            {
                Period = 1,
                Volume = 42.42,
            };

            var mockAdapter = new Mock<IPowerServiceAdapter>();
            mockAdapter.Setup(adapter => adapter.GetTradesAsync(date))
                       .ReturnsAsync(new[]
                        {
                            new PowerTradeDto
                            {
                                Date = date,
                                Periods = new []
                                {
                                    period,
                                }
                            },
                        });

            var sut = new PositionService(mockAdapter.Object);

            var result = await sut.GetPositionAsync(date);
            var isSuccessful = false;

            result.IfSucc(pos =>
            {
                isSuccessful = true;

                pos.HasPositions.Should().BeTrue();
                pos.Positions.Count.Should().Be(1);

                var actualTrade = pos.Positions.Single().Trade;

                actualTrade.Period.Should().Be(period.Period);
                actualTrade.Volume.Should().Be(period.Volume);
            });

            result.IsSuccess.Should().BeTrue();
            isSuccessful.Should().BeTrue();
        }
    }
}
