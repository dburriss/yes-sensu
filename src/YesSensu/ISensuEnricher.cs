using YesSensu.Messages;

namespace YesSensu
{
    public interface ISensuEnricher
    {
        void Enrich(IHaveMeta obj);
    }
}
