using System;

namespace PowerReport.Core.Entities
{
    public class Position
    {
        public string Timestamp { get; }
        public DateTime Date { get; }
        public Trade Trade { get; }

        private Position(string timestamp, DateTime date, Trade trade)
        {
            this.Timestamp = timestamp;
            this.Date = date;
            this.Trade = trade;
        }

        private static string GetTimestamp(DateTime date, int period, TimeZoneInfo timeZone, string format)
        {
            var origin = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Unspecified);
            var utcOrigin = TimeZoneInfo.ConvertTimeToUtc(origin, timeZone);
            var hourOffset = -2;
            var dateOffset = utcOrigin.Date.AddHours(hourOffset);

            var calculated = dateOffset.AddHours(period);

            return calculated.ToString(format);
        }

        public static Position Create(DateTime date, Trade trade, TimeZoneInfo timeZone, string format)
        {
            var timestamp = GetTimestamp(date, trade.Period, timeZone, format);

            return new Position(timestamp, date, trade);
        }
    }
}
