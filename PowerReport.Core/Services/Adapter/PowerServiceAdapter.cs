using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerReport.Core.Services.Adapter
{
    public class PowerServiceAdapter : IPowerServiceAdapter
    {
        private readonly IPowerService service;

        public PowerServiceAdapter(IPowerService service)
        {
            this.service = service;
        }

        public async Task<IReadOnlyList<PowerTradeDto>> GetTradesAsync(DateTime date)
        {
            var trades = await this.service.GetTradesAsync(date);

            return Map(trades);
        }

        private static IReadOnlyList<PowerTradeDto> Map(IEnumerable<PowerTrade> trades)
        {
            return trades.Select(trade => new PowerTradeDto
            {
                Date = trade.Date,
                Periods = trade.Periods.Select(period => new PowerPeriodDto
                {
                    Period = period.Period,
                    Volume = period.Volume,
                }).ToArray(),
            }).ToArray();
        }
    }
}
