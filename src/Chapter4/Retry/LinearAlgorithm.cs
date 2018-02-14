using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.Retry
{
    internal class LinearAlgorithm : IRetryAlgorithm
    {
        private readonly int _retries;
        private readonly int _delay;

        public LinearAlgorithm(int retries, int delay)
        {
            _retries = retries;
            _delay = delay;
        }

        public async Task<T> Execute<T>(Func<Task<T>> task)
        {
            var count = 0;

            while(true)
            {
                try
                {
                    return await task();
                }
                catch (RetryableException)
                {
                    count++;

                    if (count > _retries) throw;

                    await Task.Delay(_delay);
                }
            }
        }
    }
}
