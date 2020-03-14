using System;
using System.Threading.Tasks;
using LanguageExt;
using Polly;
using PowerReport.Core.Entities;
using PowerReport.Core.Exceptions;

namespace PowerReport.Core.Services
{
    public class PositionServiceProxy : IPositionService
    {
        private readonly IAsyncPolicy policy;
        private readonly IPositionService service;

        public PositionServiceProxy(IAsyncPolicy policy, IPositionService service)
        {
            this.policy = policy;
            this.service = service;
        }

        public async Task<Result<CalculatedPositionInfoLocalDate>> GetPositionAsync(DateTime date)
        {
            var policyRun = await this.policy.ExecuteAndCaptureAsync(() => this.service.GetPositionAsync(date));

            return policyRun.Outcome == OutcomeType.Successful
                ? policyRun.Result
                : new Result<CalculatedPositionInfoLocalDate>(new DomainServiceException(date, $"Failed to get power positions for date.", policyRun.FinalException));
        }
    }
}
