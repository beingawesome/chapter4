using System;

namespace Chapter4.EventStore
{
    public class ConnectionBuilder : IDisposable
    {
        private readonly ConnectionPool _pool;

        internal ConnectionBuilder()
        {
            _pool = new ConnectionPool();
        }

        public EventStoreFacade Build(string connectionString)
        {
            return new EventStoreFacade(_pool, connectionString);
        }

        public void Dispose()
        {
            _pool?.Dispose();
        }
    }
}
