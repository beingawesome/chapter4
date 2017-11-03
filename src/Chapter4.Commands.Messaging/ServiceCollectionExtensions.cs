using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chapter4.Commands.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandBus(this IServiceCollection services)
        {
            services.TryAddTransient(s => new CommandBus(s.GetRequiredService<IBusAdapter>(), s.GetRequiredService<ICommandMetadataFactory>()));

            return services;
        }
    }
}
