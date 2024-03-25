using System.Collections.Generic;
using System.Linq;

namespace SC_WEBAPISOCKET.WebSocket
{
    public static class HubConnections
    {
        public readonly static ConnectionMapping<string> Connections =
    new ConnectionMapping<string>();

        public readonly static ConnectionMapping<string> ConnectionsDevices =
    new ConnectionMapping<string>();
    }

    public class ConnectionMapping<T>
    {
        private Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetConnections()
        {
            IEnumerable<string> connections = new HashSet<string>();
            foreach(var conn in _connections.Values)
            {
                    connections = connections.Concat(conn);
            }

                return connections;
        }

        public IEnumerable<T> GetKeys()
        {
            return _connections.Keys;
        }

        public void Remove(string connectionId)
        {
            lock (_connections)
            {
                foreach (var conn in _connections)
                {
                    lock (conn.Value)
                    {
                        conn.Value.Remove(connectionId);
                    }
                    if(conn.Value.Count < 1)
                    {
                        _connections.Remove(conn.Key);
                    }
                }
            }
        }
    }
}
