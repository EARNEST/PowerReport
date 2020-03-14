using System;
using System.Collections.Generic;

namespace PowerReport.Core.Entities
{
    public class CalculatedPositionInfoLocalDate : CalculatedPositionInfo
    {
        private static readonly TimeZoneInfo GmtTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        private const string TimestampFormat = "HH:mm";

        private readonly Lazy<IReadOnlyList<Position>> positions;

        public override IReadOnlyList<Position> Positions => this.positions.Value;

        private CalculatedPositionInfoLocalDate(DateTime date, IEnumerable<Trade> trades) : base(date, trades)
        {
            this.positions = new Lazy<IReadOnlyList<Position>>(() => this.GetPositions(GmtTimeZone, TimestampFormat));
        }

        public static CalculatedPositionInfoLocalDate Create(DateTime date, IEnumerable<Trade> trades)
        {
            return new CalculatedPositionInfoLocalDate(date, trades);
        }
    }
}
