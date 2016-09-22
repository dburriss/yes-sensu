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
        public static Pacemaker Get(string host, int port, ClientType clientType = ClientType.Udp)
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
                    PaceMakers.Add(key, new Pacemaker(client));
                }
            }
            return PaceMakers[key];
        }


        public static void Close(string host, int port, ClientType clientType = ClientType.Udp)
        {
            var pacemaker = Get(host, port, clientType);
            pacemaker.Stop();
            PaceMakers.Remove(new Tuple<string, int, ClientType>(host, port, clientType));
        }
    }
}
