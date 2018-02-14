using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.Retry
{
    internal class NoRetryAlgorithm : IRetryAlgorithm
    {
        public Task<T> Execute<T>(Func<Task<T>> task)
        {
            return task();
        }
    }
}
