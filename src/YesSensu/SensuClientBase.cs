using System.Collections.Generic;
using YesSensu.Core;

namespace YesSensu
{
    /// <summary>
    /// Base abstract class for clients that send information to Sensu
    /// </summary>
    public abstract class SensuClientBase : ISensuClient
    {
        public ICollection<ISensuEnricher> Enrichers { get; }

        public abstract void Dispose();
        public abstract void Connect();
        public abstract void Send<TMessage>(TMessage message);

        public ISensuClient EnrichWith(ISensuEnricher enricher)
        {
            Enrichers.Add(enricher);
            return this;
        }

        protected void EnrichMessage(IHaveMeta message)
        {
            if (message == null)
                return;

            foreach (var enricher in Enrichers)
            {
                enricher.Enrich(message);
            }
        }

        protected SensuClientBase()
        {
            Enrichers = new List<ISensuEnricher>();
        }
    }
}