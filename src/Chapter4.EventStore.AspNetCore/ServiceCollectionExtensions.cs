using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace Chapter4.EventStore.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventStoreSubscriberService(this IServiceCollection services, string connectionString)
        {
            services.AddEventStore();
            services.TryAddSingleton<EventHandlers>();
            services.AddTransient(s => GetHostedService(s, connectionString));

            return services;
        }

        private static IHostedService GetHostedService(IServiceProvider services, string connectionString)
        {
            var events = services.GetRequiredService<EventHandlers>();
            var builder = services.GetRequiredService<ConnectionBuilder>();
            var connection = builder.Build(connectionString);

            return new EventHandlerService(connection, events);
        }
    }
}
