using System;

namespace PowerReport.Core.Exceptions
{
    public class DomainServiceException : Exception
    {
        public DateTime RequestedDate { get; }

        public DomainServiceException(DateTime requestedDate, string message) : base(message)
        {
            this.RequestedDate = requestedDate;
        }

        public DomainServiceException(DateTime requestedDate, string message, Exception innerException) : base(message, innerException)
        {
            this.RequestedDate = requestedDate;
        }
    }
}
