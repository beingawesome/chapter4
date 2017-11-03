using EventStore.ClientAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.EventStore
{
    internal class ConnectionPool : IDisposable
    {
        private readonly ConcurrentDictionary<string, Task<IEventStoreConnection>> _pool = new ConcurrentDictionary<string, Task<IEventStoreConnection>>();

        public Task<IEventStoreConnection> Get(string connectionString)
        {
            return _pool.GetOrAdd(connectionString, CreateAndConnect);
        }

        private async Task<IEventStoreConnection> CreateAndConnect(string connectionString)
        {
            var connection = EventStoreConnection.Create(connectionString);

            await connection.ConnectAsync();

            return connection;
        }

        public void Dispose()
        {
            var connections = _pool.Values.ToArray();

            Task.WaitAll(connections);

            foreach (var connection in connections)
            {
                try
                {
                    connection.Result?.Close();
                    connection.Result?.Dispose();
                }
                catch
                {
                }
            }
        }
    }
}
