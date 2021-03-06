using Chapter4.Queries.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chapter4.Queries.Messaging
{
    internal class QueryHandlers
    {
        private readonly IServiceProvider _services;

        public QueryHandlers(IServiceProvider services)
        {
            _services = services;
        }

        public IQueryHandler<TQuery, TResult> Build<TQuery, TResult>()
            where TQuery : Query<TResult>
        {
            return _services.GetService<IQueryHandler<TQuery, TResult>>();
        }
    }
}
