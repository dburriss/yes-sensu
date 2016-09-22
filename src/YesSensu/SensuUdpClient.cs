using System.Net.Sockets;
using System.Text;
using YesSensu.Messages;

namespace YesSensu
{
    public class SensuUdpClient : SensuClientBase
    {
        private readonly string _host;
        private readonly int _port;
        private UdpClient _client;

        public SensuUdpClient(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public override void Connect()
        {
            _client = new UdpClient();
        }

        public override void Send<TMessage>(TMessage message)
        {
            EnrichMessage(message as IHaveMeta);
            var msg = LowercaseJsonSerializer.SerializeObject(message);
            byte[] toBytes = Encoding.ASCII.GetBytes(msg);
            _client.SendAsync(toBytes, toBytes.Length, _host, _port);
        }

        public override void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
#if COREFX
                _client?.Dispose();
#endif
#if NET
                _client?.Close();
#endif
            }
        }
    }
}
