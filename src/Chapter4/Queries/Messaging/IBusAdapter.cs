using Chapter4.Metadata;
using System.Threading.Tasks;

namespace Chapter4.Queries.Messaging
{
    public interface IBusAdapter
    {
        Task<TResult> ExecuteAsync<TQuery, TResult>(TQuery query, IReadOnlyMetadata metadata)
               where TQuery : Query<TResult>;
    }
}
