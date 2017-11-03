using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EventSourcing
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(long current, long expected) : base($"Version was {current}, expected {expected}.") { }
    }
}
