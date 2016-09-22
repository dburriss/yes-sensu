using System;
using System.Collections.Generic;

namespace YesSensu
{
    public static class SensuNinja
    {
        static readonly IDictionary<Tuple<string, int, ClientType>, Pacemaker> PaceMakers = new Dictionary<Tuple<string, int, ClientType>, Pacemaker>();

        public static Pacemaker Get(string host, int port, ClientType clientType = ClientType.UDP)
        {
            if(string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));

            var key = new Tuple<string, int, ClientType>(host, port, clientType);
            if (!PaceMakers.ContainsKey(key))
            {
                Func<ISensuClient> udp = () => new SensuUdpClient(host, port);
                Func<ISensuClient> tcp = () => new SensuTcpClient(host, port);
                ISensuClient client = clientType == ClientType.UDP ? udp() : tcp();
                PaceMakers.Add(key, new Pacemaker(client));
            }

            return PaceMakers[key];
        }


        public static void Close(string host, int port, ClientType clientType = ClientType.UDP)
        {
            var pacemaker = Get(host, port, clientType);
            pacemaker.Stop();
            PaceMakers.Remove(new Tuple<string, int, ClientType>(host, port, clientType));
        }
    }


    public enum ClientType
    {
        UDP, TCP
    }
}
