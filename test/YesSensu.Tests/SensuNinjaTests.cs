using System;
using Xunit;
using YesSensu.Core;
using YesSensu.Enrichers;

namespace YesSensu.Tests
{
    public class SensuNinjaTests
    {
        [Fact]
        public void Get_WithNullOrEmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SensuNinja.Get(null, 0));
            Assert.Throws<ArgumentNullException>(() => SensuNinja.Get("", 0));
        }

        [Fact]
        public void Get_APacemaker_ReturnsAPacemaker()
        {
            var host = "host";
            var port = 1;
            var pacemaker = SensuNinja.Get(host, port);
            Assert.NotNull(pacemaker);
            Assert.IsType<Pacemaker>(pacemaker);
            SensuNinja.Kill(host, port);
        }

        [Fact]
        public void Get_APacemakerWithTcpClientParam_PacemakerHasTcpClient()
        {
            var host = "host";
            var port = 1;
            var protocol = ClientType.Tcp;
            var pacemaker = SensuNinja.Get(host, port, protocol);
            Assert.Equal(typeof(SensuTcpClient), pacemaker.Client.GetType());
            SensuNinja.Kill(host, port, protocol);
        }

        [Fact]
        public void Get_AUdpPacemakerWithEnrichers_ClientContainsEnrichers()
        {
            var host = "host";
            var port = 1;
            var protocol = ClientType.Udp;
            var pacemaker = SensuNinja.Get(host, port, protocol, new HostInfoEnricher(), new AssemblyInfoEnricher());
            Assert.Equal(2, pacemaker.Client.Enrichers.Count);
            SensuNinja.Kill(host, port, protocol);
        }

        [Fact]
        public void Get_ATcpPacemakerWithEnrichers_ClientContainsEnrichers()
        {
            var host = "host";
            var port = 1;
            var protocol = ClientType.Tcp;
            var pacemaker = SensuNinja.Get(host, port, protocol, new HostInfoEnricher(), new AssemblyInfoEnricher());
            Assert.Equal(2, pacemaker.Client.Enrichers.Count);
            SensuNinja.Kill(host, port, protocol);
        }
    }
}
