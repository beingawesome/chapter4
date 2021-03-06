﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chapter4.EventSourcing
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventSourcing(this IServiceCollection services)
        {
            services.AddTransient<AggregateRepository>();

            return services;
        }
    }
}
