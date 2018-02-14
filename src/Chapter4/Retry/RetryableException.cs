using System;

namespace Chapter4.Retry
{
    public abstract class RetryableException : Exception
    {
        public RetryableException() : base() { }
        public RetryableException(string message) : base(message) { }
        public RetryableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
