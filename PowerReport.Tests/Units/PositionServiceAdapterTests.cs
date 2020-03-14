using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PowerReport.Core.Services.Adapter;
using Services;

namespace PowerReport.Tests.Units
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    [Category(nameof(PowerServiceAdapter))]
    public class PositionServiceAdapterTests
    {
        [Test]
        public async Task Returns_Empty_Trades_When_Without_Data()
        {
            var mockService = new Mock<IPowerService>();
            mockService.Setup(svc => svc.GetTradesAsync(It.IsAny<DateTime>()))
                       .ReturnsAsync(Enumerable.Empty<PowerTrade>());

            var sut = new PowerServiceAdapter(mockService.Object);

            var trades = await sut.GetTradesAsync(It.IsAny<DateTime>());

            trades.Should().BeEmpty();
        }

        [Test]
        public async Task Returns_All_Available_Trades()
        {
            var inputDate = DateTime.UtcNow;

            var availableTrade1 = PowerTrade.Create(inputDate, 0);
            var availableTrade2 = PowerTrade.Create(inputDate, 5);
            var availableTrades = new[]
            {
                availableTrade1,
                availableTrade2,
            };

            var mockService = new Mock<IPowerService>();
            mockService.Setup(svc => svc.GetTradesAsync(inputDate))
                       .ReturnsAsync(availableTrades);

            var sut = new PowerServiceAdapter(mockService.Object);

            var trades = await sut.GetTradesAsync(inputDate);

            trades.Count.Should().Be(availableTrades.Length, "Service and Adapter DTOs count must match.");
        }

        [Test]
        public async Task Returns_Same_Structured_Trades()
        {
            var inputDate = DateTime.UtcNow;

            var availableTrade1 = PowerTrade.Create(inputDate, 0);
            var availableTrades = new[]
            {
                availableTrade1,
            };

            var mockService = new Mock<IPowerService>();
            mockService.Setup(svc => svc.GetTradesAsync(inputDate))
                       .ReturnsAsync(availableTrades);

            var sut = new PowerServiceAdapter(mockService.Object);

            var trades = await sut.GetTradesAsync(inputDate);

            var actualTrade1 = trades.Single();
            actualTrade1.Date.Should().Be(inputDate, "Service and Adapter DTOs must have the same date.");
            actualTrade1.Periods.Length.Should().Be(availableTrade1.Periods.Length, "Adapter DTO period counter must match Service DTO");
        }

        [Test]
        public async Task Returns_Correctly_Mapped_Periods_In_Trades()
        {
            var inputDate = DateTime.UtcNow;

            var availableTrade1 = PowerTrade.Create(inputDate, 5);
            var availableTrades = new[]
            {
                availableTrade1,
            };

            var mockService = new Mock<IPowerService>();
            mockService.Setup(svc => svc.GetTradesAsync(inputDate))
                       .ReturnsAsync(availableTrades);

            var sut = new PowerServiceAdapter(mockService.Object);

            var trades = await sut.GetTradesAsync(inputDate);

            var actualTrade1 = trades.Single();
            actualTrade1.Date.Should().Be(inputDate, "Service and Adapter DTOs must have the same date.");
            actualTrade1.Periods.Length.Should().Be(5, "Adapter DTO period counter must match Service DTO");

            var actualPeriod1 = actualTrade1.Periods.Select(p => new
            {
                P = p.Period,
                V = p.Volume
            }).ToArray();

            var expectedPeriod1 = availableTrade1.Periods.Select(p => new
            {
                P = p.Period,
                V = p.Volume
            }).ToArray();

            actualPeriod1.ShouldBeEquivalentTo(expectedPeriod1);
        }
    }
}
