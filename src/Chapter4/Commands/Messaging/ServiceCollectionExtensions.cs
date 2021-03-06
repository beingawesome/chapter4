using Chapter4.Retry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.Commands.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandBus(this IServiceCollection services)
        {
            services.TryAddSingleton<CommandMetadataAccessor>();
            services.TryAddTransient(Build);

            return services;
        }

        private static CommandBus Build(IServiceProvider services)
        {
            var bus = services.GetRequiredService<IBusAdapter>();
            var metadata = services.GetRequiredService<ICommandMetadataFactory>();
            var accessor = services.GetRequiredService<CommandMetadataAccessor>();
            var retry = services.GetRequiredService<IRetryAlgorithm>();

            return new CommandBus(bus, metadata, accessor, retry);
        }
    }
}
