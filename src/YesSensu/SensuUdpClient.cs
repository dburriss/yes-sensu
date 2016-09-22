using System.Net.Sockets;
using System.Text;

namespace YesSensu
{
    public class SensuUdpClient : ISensuClient
    {
        private readonly string _host;
        private readonly int _port;
        private UdpClient _client;

        public SensuUdpClient(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public virtual void Connect()
        {
            _client = new UdpClient();
        }

        public virtual void Send<TMessage>(TMessage message)
        {
            var msg = LowercaseJsonSerializer.SerializeObject(message);
            byte[] toBytes = Encoding.ASCII.GetBytes(msg);
            _client.SendAsync(toBytes, toBytes.Length, _host, _port);
        }

        public virtual void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                _client?.Dispose();
            }
        }
    }
}
