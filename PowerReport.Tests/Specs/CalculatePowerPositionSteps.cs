using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PowerReport.Core.Entities;
using PowerReport.Core.Services;
using PowerReport.Core.Services.Adapter;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace PowerReport.Tests.Specs
{
    [Binding]
    public sealed class CalculatePowerPositionSteps
    {
        private class SpecDataIn
        {
            public DateTime Date { get; set; }
            public int Period { get; set; }
            public double Volume { get; set; }
        }

        private class SpecDataOut
        {
            public string LocalTime { get; set; }
            public double Volume { get; set; }
        }

        private readonly List<Trade> dataInput;
        private readonly IPositionService service;
        private CalculatedPositionInfoLocalDate dataOutput;

        public CalculatePowerPositionSteps()
        {
            this.dataInput = new List<Trade>();
            var mockAdapter = new Mock<IPowerServiceAdapter>();

            mockAdapter.Setup(adapter => adapter.GetTradesAsync(It.IsAny<DateTime>()))
                       .ReturnsAsync((Func<DateTime, IReadOnlyList<PowerTradeDto>>)this.ConvertToTradeDtos);

            this.service = new PositionService(mockAdapter.Object);
        }

        [Given("power trades details")]
        public void GivenPowerTradesDetails(Table table)
        {
            var trades = table
                        .CreateSet<SpecDataIn>()
                        .Select(d => Trade.Create(d.Period, d.Volume));
            this.dataInput.AddRange(trades);
        }

        [When("calculating power position")]
        public async Task WhenCalculatingPowerPosition()
        {
            var result = await this.service.GetPositionAsync(DateTime.UtcNow);
            result.IfSucc(pos => this.dataOutput = pos);
        }

        [Then("power position details are")]
        public void ThenPowerPositionDetailsAre(Table table)
        {
            var expectedPositions = table.CreateSet<SpecDataOut>().ToArray();
            var actualPositions = this.dataOutput.Positions.Select(t => new SpecDataOut
            {
                LocalTime = t.Timestamp,
                Volume = t.Trade.Volume,
            }).ToArray();

            actualPositions.ShouldBeEquivalentTo(expectedPositions);
        }

        private IReadOnlyList<PowerTradeDto> ConvertToTradeDtos(DateTime date)
        {
            return this.dataInput.Select(trade => new PowerTradeDto
            {
                Date = date,
                Periods = new[]
                {
                    new PowerPeriodDto
                    {
                        Period = trade.Period,
                        Volume = trade.Volume,
                    }
                }
            }).ToArray();
        }
    }
}
