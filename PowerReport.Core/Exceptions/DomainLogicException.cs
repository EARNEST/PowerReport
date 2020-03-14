using System;

namespace PowerReport.Core.Exceptions
{
    public class DomainLogicException : Exception
    {
        public DomainLogicException(string message) : base(message) { }
    }
}
