using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.Retry
{
    public interface IRetryAlgorithm
    {
        Task<T> Execute<T>(Func<Task<T>> task);
    }
}
