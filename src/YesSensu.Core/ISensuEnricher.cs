namespace YesSensu.Core
{
    public interface ISensuEnricher
    {
        void Enrich(IHaveMeta obj);
    }
}
