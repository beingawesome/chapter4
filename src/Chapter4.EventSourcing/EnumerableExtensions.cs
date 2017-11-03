using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter4.EventSourcing
{
    internal static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
    }
}
