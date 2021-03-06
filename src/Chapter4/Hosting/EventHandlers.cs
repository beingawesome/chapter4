using Chapter4.Events;
using Chapter4.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Chapter4.EventStore.AspNetCore
{
    internal class EventHandlers
    {
        private readonly IServiceProvider _services;

        public EventHandlers(IServiceProvider services)
        {
            _services = services;
        }

        public IEnumerable<IEventHandler<TEvent>> Build<TEvent>()
            where TEvent : Event
        {
            return _services.GetServices<IEventHandler<TEvent>>();
        }
    }
}
