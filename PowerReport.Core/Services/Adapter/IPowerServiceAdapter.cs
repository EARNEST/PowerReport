using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerReport.Core.Services.Adapter
{

    public interface IPowerServiceAdapter
    {
        Task<IReadOnlyList<PowerTradeDto>> GetTradesAsync(DateTime date);
    }
}
