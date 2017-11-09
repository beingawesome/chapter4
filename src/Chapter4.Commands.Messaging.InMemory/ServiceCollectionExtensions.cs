using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chapter4.Commands.Messaging
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
