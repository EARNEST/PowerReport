using System;
using System.Linq;
using Castle.Core.Internal;
using FluentAssertions;
using NUnit.Framework;
using PowerReport.Core.Entities;
using PowerReport.Core.Exceptions;

namespace PowerReport.Tests.Specs
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class DomainTests
    {
        [Test]
        public void No_Positions_Without_Trades()
        {
            var agg = CalculatedPositionInfoLocalDate.Create(DateTime.UtcNow, Enumerable.Empty<Trade>());

            agg.HasPositions.Should().BeFalse("Has no positions when no trades provided.");
        }

        [Test]
        public void Trade_Not_Accept_Period_Of_Lt_1()
        {
            Action act = () => Trade.Create(0, 0);

            act.ShouldThrow<DomainLogicException>();
        }

        [Test]
        public void Trade_Not_Accept_Volume_Of_NaN()
        {
            Action[] acts =
            {
                () => Trade.Create(1, double.NaN),
                () => Trade.Create(1, double.NegativeInfinity),
                () => Trade.Create(1, double.PositiveInfinity),
            };

            acts.ForEach(act => act.ShouldThrow<DomainLogicException>());
        }
    }
}
