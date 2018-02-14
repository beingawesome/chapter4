using Chapter4.Events.Store;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.EventStore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services)
        {
            services.TryAddSingleton(s => new ConnectionBuilder());

            return services;
        }
        
        public static IServiceCollection AddEventStoreEventStore(this IServiceCollection services, string connectionString)
        {
            services.AddEventStore();
            services.TryAddSingleton< EventMetadataFactory>();
            services.TryAddTransient(s => GetEventStore(s, connectionString));

            return services;
        }

        private static IEventStore GetEventStore(IServiceProvider services, string connectionString)
        {
            var metadata = services.GetRequiredService<EventMetadataFactory>();
            var builder = services.GetRequiredService<ConnectionBuilder>();
            var connection = builder.Build(connectionString);

            return new EventStoreStore(connection, metadata);
        }
    }
}
