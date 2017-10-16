using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chapter4.Queries.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueryBus(this IServiceCollection services)
        {
            services.TryAddSingleton(s => new QueryBus(s.GetRequiredService<IBusAdapter>(), s.GetRequiredService<IQueryMetadataFactory>()));

            return services;
        }
    }
}
