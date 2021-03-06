using Chapter4.Metadata;
using System.Threading.Tasks;

namespace Chapter4.Queries.Handlers
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : Query<TResult>
    {
        Task<TResult> ExecuteAsync(TQuery query, IReadOnlyMetadata metadata);
    }
}
