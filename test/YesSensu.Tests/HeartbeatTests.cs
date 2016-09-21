using System;
using Xunit;
using YesSensu.Core.Messages;

namespace YesSensu.Core.Tests
{
    public class HeartbeatTests
    {
        [Fact]
        public void Ctor_WithNullOrEmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Heartbeat(null));
            Assert.Throws<ArgumentNullException>(() => new Heartbeat(""));
        }
    }
}
