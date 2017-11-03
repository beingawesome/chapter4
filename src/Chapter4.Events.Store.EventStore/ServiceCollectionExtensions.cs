using Chapter4.EventStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.Events.Store.EventStore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventStoreEventStore(this IServiceCollection services, string connectionString)
        {
            services.AddEventStore();
            services.TryAddTransient(s => GetEventStore(s, connectionString));

            return services;
        }

        private static IEventStore GetEventStore(IServiceProvider services, string connectionString)
        {
            var metadata = services.GetRequiredService<IEventMetadataFactory>();
            var builder = services.GetRequiredService<ConnectionBuilder>();
            var connection = builder.Build(connectionString);

            return new EventStoreStore(connection, metadata);
        }
    }
}
