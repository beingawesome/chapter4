using Chapter4.Retry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.Queries.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueryBus(this IServiceCollection services)
        {
            services.TryAddTransient(Build);

            return services;
        }

        public static QueryBus Build(IServiceProvider services)
        {
            var bus = services.GetRequiredService<IBusAdapter>();
            var metadata = services.GetRequiredService<IQueryMetadataFactory>();
            var retry = services.GetRequiredService<IRetryAlgorithm>();
            
            return new QueryBus(bus, metadata, retry);
        }
    }
}
