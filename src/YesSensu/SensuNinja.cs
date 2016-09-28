using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using YesSensu.Core;

namespace YesSensu
{
    public static class SensuNinja
    {
        static readonly IDictionary<Tuple<string, int, ClientType>, Pacemaker> PaceMakers = new ConcurrentDictionary<Tuple<string, int, ClientType>, Pacemaker>();
        private static readonly object Locker = new object();

        /// <summary>
        /// I return a singleton instance of a Pacemaker that can be accessed throughout the application. Key is host, port, and clientType.
        /// </summary>
        /// <see cref="Pacemaker"/>
        /// <param name="host">Sensu host address</param>
        /// <param name="port">Sensu port</param>
        /// <param name="clientType">Sensu client</param>
        /// <see cref="ClientType"/>
        /// <seealso cref="SensuUdpClient"/>
        /// <seealso cref="SensuTcpClient"/>
        /// <param name="enrichers">An array of ISensuEnricher that enrich all heartbeats sent</param>
        /// <seealso cref="ISensuEnricher"/>
        /// <returns></returns>
        public static Pacemaker Get(string host, int port, ClientType clientType = ClientType.Udp, params ISensuEnricher[] enrichers)
        {
            if(string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));

            var key = new Tuple<string, int, ClientType>(host, port, clientType);
            lock (Locker)
            { 
                if (!PaceMakers.ContainsKey(key))
                {
                    Func<ISensuClient> udp = () => new SensuUdpClient(host, port);
                    Func<ISensuClient> tcp = () => new SensuTcpClient(host, port);
                    ISensuClient client = clientType == ClientType.Udp ? udp() : tcp();
                    foreach (var sensuEnricher in enrichers)
                    {
                        client.EnrichWith(sensuEnricher);
                    }
                    PaceMakers.Add(key, new Pacemaker(client));
                }
            }
            return PaceMakers[key];
        }

        /// <summary>
        /// I kill the singleton based on host, port, and clientType
        /// </summary>
        /// <param name="host">Sensu host address</param>
        /// <param name="port">Sensu port</param>
        /// <param name="clientType">Sensu client</param>
        /// <see cref="ClientType"/>
        /// <seealso cref="SensuUdpClient"/>
        /// <seealso cref="SensuTcpClient"/>
        public static void Kill(string host, int port, ClientType clientType = ClientType.Udp)
        {
            var key = new Tuple<string, int, ClientType>(host, port, clientType);
            lock (Locker)
            {
                if (PaceMakers.ContainsKey(key))
                {
                    var pacemaker = Get(host, port, clientType);
                    pacemaker.Stop();
                    PaceMakers.Remove(new Tuple<string, int, ClientType>(host, port, clientType));
                }
            }
        }
    }
}
