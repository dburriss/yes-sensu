using System;
using Xunit;
using YesSensu.Messages;

namespace YesSensu.Tests
{
    public class HeartbeatTests
    {
        [Fact]
        public void Ctor_WithAppNameNullOrEmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Heartbeat(null));
            Assert.Throws<ArgumentNullException>(() => new Heartbeat(""));
        }

        [Fact]
        public void Ctor_AppName_SourceIsAppName()
        {
            var appName = "app_name";
            var sut = new Heartbeat(appName);
            Assert.Equal(appName, sut.Source);
        }

        [Fact]
        public void Ctor_AppName_NameIsHeartbeat()
        {
            var sut = new Heartbeat("anything");
            Assert.Equal("heartbeat", sut.Name);
        }
    }
}
