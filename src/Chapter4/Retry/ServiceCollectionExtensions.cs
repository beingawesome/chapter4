using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.Retry
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLinearRetryAlgorithm(this IServiceCollection services, int retries, int delay)
        {
            services.TryAddSingleton(new LinearAlgorithm(retries, delay));

            return services;
        }

        public static IServiceCollection AddNoRetryAlgorithm(this IServiceCollection services)
        {
            services.TryAddSingleton(new NoRetryAlgorithm());

            return services;
        }
    }
}
