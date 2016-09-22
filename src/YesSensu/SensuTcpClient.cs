using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace YesSensu
{
    public class SensuTcpClient : ISensuClient
    {
        private readonly string _host;
        private readonly int _port;
        private TcpClient _client;

        public SensuTcpClient(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public virtual void Connect()
        {
            _client = new TcpClient();
            _client.ConnectAsync(_host, _port);
            while (!_client.Connected)
            {
                Thread.Sleep(10);
            }
        }

        public virtual void Send<TMessage>(TMessage message)
        {
            var msg = LowercaseJsonSerializer.SerializeObject(message);
            byte[] toBytes = Encoding.ASCII.GetBytes(msg);
            _client.GetStream().WriteAsync(toBytes, 0, toBytes.Length);
        }

        public virtual void Dispose()
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
