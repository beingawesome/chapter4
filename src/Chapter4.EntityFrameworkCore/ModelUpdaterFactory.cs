using Chapter4.EventSourcing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chapter4.EntityFrameworkCore
{

    internal class ModelUpdaterFactory
    {
        private readonly IServiceProvider _factory;

        public ModelUpdaterFactory(IServiceProvider factory) => _factory = factory;

        public ModelUpdater<T> Get<T>() where T : AggregateRoot => _factory.GetService<ModelUpdater<T>>();
    }
}
