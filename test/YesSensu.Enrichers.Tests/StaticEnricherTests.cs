using System.Collections.Generic;
using Xunit;
using YesSensu.Core;
using YesSensu.Messages;

namespace YesSensu.Enrichers.Tests
{
    public class StaticEnricherTests
    {
        [Fact]
        public void Enrich_FromDictionaryWithX_XIsInMessageMeta()
        {
            IDictionary<string, object> data = new Dictionary<string, object>()
            {
                {"X", "anything"} 
            };
            var sut = new StaticEnricher(data);
            var message = new Ok("app_name","some_key");
            sut.Enrich(message);
            Assert.True(message.Meta.ContainsKey("X"));
            Assert.Equal("anything", message.Meta["X"]);
        }

        [Fact]
        public void Enrich_FromDictionaryWithXAndOverrideIsFalse_XIsSomething()
        {
            IDictionary<string, object> data = new Dictionary<string, object>()
            {
                {"X", "anything"}
            };
            var sut = new StaticEnricher(data);
            var message = new TestMessageWithXMeta();
            sut.Enrich(message);
            Assert.True(message.Meta.ContainsKey("X"));
            Assert.Equal("something", message.Meta["X"]);
        }

        [Fact]
        public void Enrich_FromDictionaryWithXAndOverrideIsTrue_XIsAnything()
        {
            IDictionary<string, object> data = new Dictionary<string, object>()
            {
                {"X", "anything"}
            };
            var sut = new StaticEnricher(data, true);
            var message = new TestMessageWithXMeta();
            sut.Enrich(message);
            Assert.True(message.Meta.ContainsKey("X"));
            Assert.Equal("anything", message.Meta["X"]);
        }
    }

    public class TestMessageWithXMeta : SensuBase
    {
        public TestMessageWithXMeta() : base("app_name", "some_key", Status.Ok)
        {
            Meta["X"] = "something";
        }
    }
}
