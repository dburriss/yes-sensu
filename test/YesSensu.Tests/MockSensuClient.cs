using System.Collections.Generic;
using System.Reflection;
using YesSensu.Messages;

namespace YesSensu.Tests
{
    public class MockSensuClient : SensuClientBase
    {
        public bool IsDisposed { get; private set; }
        public bool IsConnected { get; private set; }
        public ICollection<object> Messages { get; private set; }
        public bool Enriched { get; private set; }

        public MockSensuClient()
        {
            Messages = new List<object>();
        }
        public override void Dispose()
        {
            IsDisposed = true;
        }

        public override void Connect()
        {
            IsConnected = true;
        }

        public override void Send<TMessage>(TMessage message)
        {
            var metable = message as IHaveMeta;
            if (metable != null)
            {
                Enriched = true;
                EnrichMessage(metable);
            }
            Messages.Add(message);
        }
        
        public IEnumerable<ISensuEnricher> GetEnrichers()
        {
            return Enrichers;
        }
    }
}