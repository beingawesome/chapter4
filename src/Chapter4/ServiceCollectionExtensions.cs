using Chapter4.Commands.Messaging;
using Chapter4.Commands.Messaging.InMemory;
using Chapter4.EventSourcing;
using Chapter4.Queries.Messaging;
using Chapter4.Queries.Messaging.InMemory;
using Chapter4.Retry;
using Chapter4.Snapshots.Store.Null;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChapter4Core(this IServiceCollection services)
        {
            services.AddLinearRetryAlgorithm(5, 200);
            services.AddCommandBus();
            services.AddQueryBus();
            services.AddEventSourcing();

            return services;
        }

        public static IServiceCollection AddChapter4(this IServiceCollection services)
        {
            services.AddChapter4Core();
            services.AddNullSnapshotStore();
            services.AddInMemoryCommandMessaging();
            services.AddInMemoryQueryMessaging();
            
            return services;
        }
    }
}
