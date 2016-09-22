using System;

namespace YesSensu
{
    public interface ISensuClient : IDisposable
    {
        void Connect();
        void Send<TMessage>(TMessage message);
        ISensuClient EnrichWith(ISensuEnricher enricher);
    }
}