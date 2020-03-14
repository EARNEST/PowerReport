using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using PowerReport.Core.Entities;
using PowerReport.Core.Exceptions;
using PowerReport.Core.Services.Adapter;

namespace PowerReport.Core.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPowerServiceAdapter adapter;

        public PositionService(IPowerServiceAdapter adapter)
        {
            this.adapter = adapter;
        }

        public async Task<Result<CalculatedPositionInfoLocalDate>> GetPositionAsync(DateTime date)
        {
            var dtoTrades = await this.adapter.GetTradesAsync(date);

            var dates = dtoTrades.Select(t => t.Date).Distinct().ToArray();
            if (dates.Length > 1)
            {
                return new Result<CalculatedPositionInfoLocalDate>(new DomainServiceException(date, "Power trades must fall on the same date."));
            }

            var trades = Map(dtoTrades);

            return CalculatedPositionInfoLocalDate.Create(date, trades);
        }

        private static IReadOnlyList<Trade> Map(IEnumerable<PowerTradeDto> trades)
        {
            return trades
                  .SelectMany(d => d.Periods)
                  .Select(p => Trade.Create(p.Period, p.Volume))
                  .ToArray();
        }
    }
}
