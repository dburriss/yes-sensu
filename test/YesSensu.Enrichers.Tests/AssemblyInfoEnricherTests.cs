using System;
using System.Linq;
using System.Reflection;
using PhilosophicalMonkey;
using Xunit;
using YesSensu.Messages;

namespace YesSensu.Enrichers.Tests
{
    public class AssemblyInfoEnricherTests
    {
        [Fact]
        public void Enrich_FromCurrentAssebmly_ProductIsYesSensuEnrichersTests()
        {
            var assembly = Reflect.OnTypes.GetAssembly(typeof(AssemblyInfoEnricherTests));
            var sut = new AssemblyInfoEnricher(assembly);
            var message = new Ok("app_name","some_key");
            sut.Enrich(message);
            Assert.True(message.Meta.ContainsKey("Product"));
            Assert.Equal("YesSensu.Enrichers.Tests", message.Meta["Product"]);
        }

    }
}
