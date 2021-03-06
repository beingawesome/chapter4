using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chapter4.Queries.Messaging.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryQueryMessaging(this IServiceCollection services)
        {
            services.AddQueryBus();

            services.TryAddTransient<QueryHandlers>();
            services.TryAddTransient<IBusAdapter, InMemoryBusAdapter>();

            return services;
        }
    }
}
