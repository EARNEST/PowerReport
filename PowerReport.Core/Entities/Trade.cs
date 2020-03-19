using PowerReport.Core.Exceptions;

namespace PowerReport.Core.Entities
{
    public class Trade
    {
        public int Period { get; }
        public double Volume { get; }

        private Trade(int period, double volume)
        {
            this.Period = period;
            this.Volume = volume;
        }

        public static Trade Create(int period, double volume)
        {
            if (period < 1)
            {
                throw new DomainLogicException($"Period is out of range. Period: {period}");
            }

            if (double.IsNaN(volume) || double.IsInfinity(volume))
            {
                throw new DomainLogicException($"Volume is invalid. Volume: {volume}");
            }

            return new Trade(period, volume);
        }
    }
}
