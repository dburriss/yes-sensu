using System;
using System.Collections.Generic;

namespace YesSensu.Core
{
    public interface ISensuClient : IDisposable
    {
        void Connect();
        void Send<TMessage>(TMessage message);
        ISensuClient EnrichWith(ISensuEnricher enricher);
        ICollection<ISensuEnricher> Enrichers { get; }
    }
}