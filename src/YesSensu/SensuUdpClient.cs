using System;
using System.Net.Sockets;
using System.Text;
using YesSensu.Core;

namespace YesSensu
{
    public class SensuUdpClient : SensuClientBase
    {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _throwOnConnectionError;
        private UdpClient _client;

        public SensuUdpClient(string host, int port, bool throwOnConnectionError = false)
        {
            _host = host;
            _port = port;
            _throwOnConnectionError = throwOnConnectionError;
        }

        public override void Connect()
        {
            try
            {
                _client = new UdpClient();
            }
            catch (Exception)
            {
                if (_throwOnConnectionError)
                    throw;
            }

        }

        public override void Send<TMessage>(TMessage message)
        {
            if (_client == null)
                throw new InvalidOperationException("Udp client is null. Did you call Connect()?");
            try
            {
                EnrichMessage(message as IHaveMeta);
                var msg = LowercaseJsonSerializer.SerializeObject(message);
                byte[] toBytes = Encoding.ASCII.GetBytes(msg);
                _client.SendAsync(toBytes, toBytes.Length, _host, _port);
            }
            catch (Exception)
            {
                if (_throwOnConnectionError)
                    throw;
            }

        }

        public override void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                try
                {
#if COREFX
                    _client?.Dispose();
#endif
#if NET
                    _client?.Close();
#endif
                }
                catch (Exception)
                {
                    if(_throwOnConnectionError)
                        throw;
                }

            }
        }
    }
}
