using Chapter4.Metadata;
using Chapter4.Retry;
using System.Reflection;
using System.Threading.Tasks;

namespace Chapter4.Queries.Messaging
{
    public class QueryBus
    {
        private static readonly MethodInfo ExecuteAsyncMethod = typeof(IBusAdapter).GetMethod(nameof(IBusAdapter.ExecuteAsync));
        private static readonly MethodInfo CreateMethod = typeof(IQueryMetadataFactory).GetMethod(nameof(IQueryMetadataFactory.Create));

        private readonly IBusAdapter _bus;
        private readonly IQueryMetadataFactory _metadata;
        private readonly IRetryAlgorithm _retry;

        internal QueryBus(IBusAdapter bus, IQueryMetadataFactory metadata, IRetryAlgorithm retry)
        {
            _bus = bus;
            _metadata = metadata;
            _retry = retry;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Query<TResult> query)
        {
            var create = CreateMethod.MakeGenericMethod(query.GetType(), typeof(TResult));
            var execute = ExecuteAsyncMethod.MakeGenericMethod(query.GetType(), typeof(TResult));

            var metadata = (IMetadata)create.Invoke(_metadata, new[] { query });
            
            return await _retry.Execute(() => (Task<TResult>)execute.Invoke(_bus, new object[] { query, metadata })).ConfigureAwait(false);
        }
    }
}
