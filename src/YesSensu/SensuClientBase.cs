using System.Collections.Generic;
using YesSensu.Messages;

namespace YesSensu
{
    public abstract class SensuClientBase : ISensuClient
    {
        protected ICollection<ISensuEnricher> Enrichers = new List<ISensuEnricher>();

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
    }
}