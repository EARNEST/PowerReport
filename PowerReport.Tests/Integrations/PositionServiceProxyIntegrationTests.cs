using System;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using Moq;
using NUnit.Framework;
using PowerReport.Core.Entities;
using PowerReport.Core.Factories;
using PowerReport.Core.Services;

namespace PowerReport.Tests.Integrations
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    [Category(TestCategory.FastIntegration)]
    [Category(nameof(PositionServiceProxy))]
    public class PositionServiceProxyIntegrationTests
    {
        [Test]
        public async Task Retries_On_Failure_Until_Timeout_Reached()
        {
            var mockService = new Mock<IPositionService>();
            mockService.Setup(svc => svc.GetPositionAsync(It.IsAny<DateTime>()))
                       .ThrowsAsync(new Exception("Simulated failure during test."));

            var policy = PositionServiceProxyFactory.CreatePolicy(TimeSpan.FromSeconds(1));

            var sut = new PositionServiceProxy(policy, mockService.Object);

            await sut.GetPositionAsync(It.IsAny<DateTime>());

            mockService.Verify(svc => svc.GetPositionAsync(It.IsAny<DateTime>()), Times.AtLeast(2), "Proxy must retries on service failure.");
        }

        [Test]
        public async Task Terminates_On_Exceeding_Timeout()
        {
            var maxTimeout = TimeSpan.FromSeconds(1);
            var delayedTimeout = TimeSpan.FromSeconds(2);

            Func<DateTime, Task<Result<CalculatedPositionInfoLocalDate>>> delayedResponse = async date =>
            {
                await Task.Delay(delayedTimeout);

                return Result<CalculatedPositionInfoLocalDate>.Bottom;
            };

            var mockService = new Mock<IPositionService>();
            mockService.Setup(svc => svc.GetPositionAsync(It.IsAny<DateTime>()))
                       .Returns(delayedResponse);

            var policy = PositionServiceProxyFactory.CreatePolicy(maxTimeout);

            var sut = new PositionServiceProxy(policy, mockService.Object);

            var agg = await sut.GetPositionAsync(It.IsAny<DateTime>());

            agg.IsFaulted.Should().BeTrue("Proxy must terminate on long service call.");
        }
    }
}
