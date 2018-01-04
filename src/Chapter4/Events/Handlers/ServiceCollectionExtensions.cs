using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chapter4.Events.Handlers.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Type HandlerType = typeof(IEventHandler<>);

        public static IServiceCollection AddEventHandlersFromAssembly<T>(this IServiceCollection services)
        {
            // TODO: Add marker to service collection to prevent duplicate calls

            return services.AddEventHandlersFromAssembly(typeof(T).Assembly);
        }

        private static IServiceCollection AddEventHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            foreach (var mapping in GetHandlerMappings(assembly))
            {
                services.AddTransient(mapping.Interface, mapping.Type);
            }

            return services;

        }

        private static IEnumerable<(Type Interface, Type Type)> GetHandlerMappings(Assembly asm)
        {
            return from type in asm.DefinedTypes
                   from i in type.ImplementedInterfaces
                   let detail = i.GetTypeInfo()
                   where detail.IsGenericType
                   let def = detail.GetGenericTypeDefinition()
                   where def == HandlerType
                   select (i, type.AsType());
        }
    }
}
