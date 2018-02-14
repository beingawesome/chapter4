using Chapter4.Retry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EventSourcing
{
    public class ConcurrencyException : RetryableException
    {
        public ConcurrencyException(long current, long expected) : base($"Version was {current}, expected {expected}.") { }
    }
}
