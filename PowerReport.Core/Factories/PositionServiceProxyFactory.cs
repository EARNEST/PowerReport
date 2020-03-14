using System;
using Polly;
using Polly.Timeout;
using PowerReport.Core.Services;
using PowerReport.Core.Services.Adapter;
using Services;

namespace PowerReport.Core.Factories
{
    public class PositionServiceProxyFactory
    {
        public static IPositionService Create(TimeSpan maxTimeOut)
        {
            var policy = CreatePolicy(maxTimeOut);

            var adapter = new PowerServiceAdapter(new PowerService());
            var service = new PositionService(adapter);

            return new PositionServiceProxy(policy, service);
        }

        public static IAsyncPolicy CreatePolicy(TimeSpan maxTimeout)
        {
            var policyTimeout = Policy.TimeoutAsync(
                maxTimeout,
                TimeoutStrategy.Pessimistic,
                onTimeoutAsync: (context, timespan, task) => throw new TimeoutException($"Timeout occurred. Time taken: {timespan}."));

            var policyRetry = Policy.Handle<Exception>().RetryForeverAsync();

            var policyWrap = policyTimeout.WrapAsync(policyRetry);

            return policyWrap;
        }
    }
}
