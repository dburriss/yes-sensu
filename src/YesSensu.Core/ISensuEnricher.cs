namespace YesSensu.Core
{
    public interface ISensuEnricher
    {
        /// <summary>
        /// I enrich the message with metadata
        /// </summary>
        /// <param name="obj">Message to be enriched</param>
        void Enrich(IHaveMeta obj);
    }
}
