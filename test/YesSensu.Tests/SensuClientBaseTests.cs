using System.Linq;
using Xunit;
using YesSensu.Core;
using YesSensu.Enrichers;
using YesSensu.Messages;

namespace YesSensu.Tests
{
    public class SensuClientBaseTests
    {
        [Fact]
        public void EnrichWith_WithAnEnricher_AddsEnricherToClient()
        {
            ISensuEnricher enricher = new HostInfoEnricher();
            var sut = new MockSensuClient();
            sut.EnrichWith(enricher);
            var enrichers = sut.GetEnrichers();
            Assert.Contains(enricher, enrichers);
        }

        [Fact]
        public void Send_OnNotIHaveMeta_DoesNotCallEnrichMessage()
        {
            ISensuEnricher enricher = new HostInfoEnricher();
            var sut = new MockSensuClient();
            sut.EnrichWith(enricher);
            sut.Send(new {});
            Assert.False(sut.Enriched);
        }

        [Fact]
        public void Send_OnIHaveMeta_DoesCallEnrichMessage()
        {
            ISensuEnricher enricher = new HostInfoEnricher();
            var sut = new MockSensuClient();
            sut.EnrichWith(enricher);
            sut.Send(new Ok("app_name", "some_metric"));
            Assert.True(sut.Enriched);
        }

        [Fact]
        public void EnrichMessage_OnIHaveMeta_AddsMeta()
        {
            ISensuEnricher enricher = new HostInfoEnricher();
            var sut = new MockSensuClient();
            sut.EnrichWith(enricher);
            var message = new Ok("app_name", "some_metric");
            sut.Send(message);
            Assert.True(message.Meta.ContainsKey("host_name"));
        }
    }
}
