using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerReport.Core.Entities
{
    public abstract class CalculatedPositionInfo
    {
        private readonly IReadOnlyList<Trade> trades;

        public DateTime Date { get; }
        public abstract IReadOnlyList<Position> Positions { get; }

        public bool HasPositions => this.Positions.Any();

        protected CalculatedPositionInfo(DateTime date, IEnumerable<Trade> trades)
        {
            this.Date = date;
            this.trades = trades.ToArray();
        }

        protected IReadOnlyList<Position> GetPositions(TimeZoneInfo gmtTimeZone, string timestampFormat)
        {
            return this.trades
                       .GroupBy(seq => seq.Period)
                       .Select(group =>
                        {
                            var totalVolume = @group.Sum(x => x.Volume);
                            var trade = Trade.Create(group.Key, totalVolume);

                            return Position.Create(this.Date, trade, gmtTimeZone, timestampFormat);
                        })
                       .ToArray();
        }
    }
}
