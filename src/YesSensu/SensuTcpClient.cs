using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using YesSensu.Core;

namespace YesSensu
{
    public class SensuTcpClient : SensuClientBase
    {
        private const int Timeout = 5*1000;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _throwOnConnectionError;
        private TcpClient _client;

        public SensuTcpClient(string host, int port, bool throwOnConnectionError = false)
        {
            _host = host;
            _port = port;
            _throwOnConnectionError = throwOnConnectionError;
        }

        public override void Connect()
        {
            var connectTime = 0;
            _client = new TcpClient();
            _client.ConnectAsync(_host, _port);
            while (!_client.Connected && connectTime < Timeout)
            {
                var sleepTime = 10;
                Thread.Sleep(sleepTime);
                connectTime += sleepTime;
            }
            if(connectTime >= Timeout && !_client.Connected && _throwOnConnectionError)
                throw new TimeoutException($"Failed to connect to server within {Timeout} second timeout period.");
        }

        public override void Send<TMessage>(TMessage message)
        {
            EnrichMessage(message as IHaveMeta);
            var msg = LowercaseJsonSerializer.SerializeObject(message);
            byte[] toBytes = Encoding.ASCII.GetBytes(msg);
            _client.GetStream().WriteAsync(toBytes, 0, toBytes.Length);
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
