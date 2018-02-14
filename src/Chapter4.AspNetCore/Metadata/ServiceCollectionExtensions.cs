using Chapter4.Commands.Messaging;
using Chapter4.Queries.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Chapter4.Metadata
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMetadataFactories(this IServiceCollection services, Action<MetadataOptions> options)
        {
            services.Configure(options);
            services.TryAddTransient<MetadataFactory>();
            services.TryAddTransient<ICommandMetadataFactory, MetadataFactory>();
            services.TryAddTransient<IQueryMetadataFactory, MetadataFactory>();

            return services;
        }        
    }
}
