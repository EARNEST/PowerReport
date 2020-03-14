using System;

namespace PowerReport.Core.Services.Adapter
{
    public class PowerTradeDto
    {
        public DateTime Date { get; set; }
        public PowerPeriodDto[] Periods { get; set; }
    }
}
