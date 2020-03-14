using System;
using System.Threading.Tasks;
using LanguageExt;
using PowerReport.Core.Entities;

namespace PowerReport.Core.Services
{
    public interface IPositionService
    {
        Task<Result<CalculatedPositionInfoLocalDate>> GetPositionAsync(DateTime date);
    }
}
