using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace YesSensu
{
    public class SensuUdpClient : ISensuClient
    {
        private readonly string _host;
        private readonly int _port;
        private UdpClient _client;
        //private static IDictionary<Tuple<string, int>, ClientContainer> _clients 
        //    = new ConcurrentDictionary<Tuple<string, int>, ClientContainer>();

        public SensuUdpClient(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public virtual void Connect()
        {
            _client = new UdpClient();
            ////TODO: lock this block
            //if (!_clients.ContainsKey(Key()))
            //{
            //    _clients.Add(Key(), new ClientContainer(new UdpClient()));
            //}
            //_clients[Key()].Increment();
        }

        //private Tuple<string, int> Key()
        //{
        //    return new Tuple<string, int>(_host, _port);
        //}

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
            ////TODO: lock this block
            //if (dispose && _clients.ContainsKey(Key()))
            //{
            //    var c = _clients[Key()];
            //    c.Decrement();
            //    if(c.Count == 0)
            //        _client?.Dispose();
            //}
        }
    }

    //internal class ClientContainer
    //{
    //    public int Count { get; private set; }
    //    public UdpClient Client { get; private set; }

    //    public ClientContainer(UdpClient client)
    //    {
    //        Client = client;
    //        Count = 1;
    //    }
    //    public void Increment()
    //    {
    //        Count++;
    //    }

    //    public void Decrement()
    //    {
    //        Count--;
    //    }
    //}
}
