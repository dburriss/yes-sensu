using System;
using System.Collections.Generic;

namespace YesSensu.Core
{
    public interface ISensuClient : IDisposable
    {
        /// <summary>
        /// Setup the connection to the client
        /// </summary>
        /// <seealso cref="ISensuClient"/>
        /// <remarks>What exactly that means depends on the client. For TCP this creates a connection. For UDP not.</remarks>
        void Connect();

        /// <summary>
        /// I send a message to Sensu
        /// </summary>
        /// <typeparam name="TMessage">Generic argument of the message instance type</typeparam>
        /// <param name="message">An optional message to send with the update</param>
        void Send<TMessage>(TMessage message);

        /// <summary>
        /// Adds an enricher which is used to add Meta data to each message that this client sends
        /// </summary>
        /// <param name="enricher">The enricher used to add meta data</param>
        /// <see cref="ISensuEnricher"/>
        /// <seealso cref="IHaveMeta"/>
        /// <returns></returns>
        ISensuClient EnrichWith(ISensuEnricher enricher);

        /// <summary>
        /// Collection of ISensuEnricher on the client that will be applied to all messages sent
        /// </summary>
        /// <see cref="ISensuEnricher"/>
        /// <seealso cref="IHaveMeta"/>
        ICollection<ISensuEnricher> Enrichers { get; }
    }
}