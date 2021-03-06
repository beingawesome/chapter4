using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.Commands.Messaging.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryCommandMessaging(this IServiceCollection services)
        {
            services.AddCommandBus();

            services.TryAddTransient<CommandHandlers>();
            services.TryAddTransient<IBusAdapter, InMemoryBusAdapter>();

            return services;
        }
    }
}
