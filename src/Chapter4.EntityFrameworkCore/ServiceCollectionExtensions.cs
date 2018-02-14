using Chapter4.EntityFrameworkCore;
using Chapter4.Events.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.EventStore
{
    public static class EntityFrameworkCore
    {
        public static IServiceCollection AddEntityFrameworkEventStore<T>(this IServiceCollection services)
            where T: DbContext
        {
            services.TryAddSingleton<ModelUpdaterFactory>();
            services.TryAddSingleton<EventMetadataFactory>();
            services.TryAddTransient(GetEventStore<T>);

            return services;
        }

        private static IEventStore GetEventStore<T>(IServiceProvider services) where T : DbContext
        {
            var updater = services.GetRequiredService<ModelUpdaterFactory>();
            var metadata = services.GetRequiredService<EventMetadataFactory>();
            var context = services.GetRequiredService<T>();

            return new EntityFrameworkEventStore(context, metadata, updater);
        }
    }
}
