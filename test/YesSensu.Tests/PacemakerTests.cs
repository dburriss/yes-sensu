using System;
using System.Threading;
using Xunit;
using YesSensu.Messages;

namespace YesSensu.Core.Tests
{
    public class PacemakerTests : IDisposable
    {
        private Pacemaker _sut;

        [Fact]
        public void Ctor_WithNoSensuClient_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Pacemaker(null));
        }

        [Fact]
        public void Start_WithNoHeartbeat_ThrowsArgumentNullException()
        {
            var client = new MockSensuClient();
            _sut = new Pacemaker(client);
            Assert.Throws<ArgumentNullException>(() => _sut.Start(null));
        }

        [Fact]
        public void Stop_WhenNotStarted_DoesNothing()
        {
            var client = new MockSensuClient();
            _sut = new Pacemaker(client);
            _sut.Stop();
        }

        [Fact]
        public void Start_WithPeriodOf1sec_Sends2MessagesIn2sec()
        {
            var client = new MockSensuClient();
            _sut = new Pacemaker(client);
            _sut.Start(new Heartbeat("testApp", 1));
            Thread.Sleep(1900);
            _sut.Stop();
            Assert.Equal(2, client.Messages.Count);
        }

        [Fact]
        public void Start_WithValidHeartbeat_Sends1stMessageWithin5ms()
        {
            var client = new MockSensuClient();
            _sut = new Pacemaker(client);
            _sut.Start(new Heartbeat("testApp"));
            Thread.Sleep(5);
            Assert.NotEmpty(client.Messages);
        }

        [Fact]
        public void Stop_WhileHeardbeatRunning_StopsHeartbeat()
        {
            var client = new MockSensuClient();
            _sut = new Pacemaker(client);
            _sut.Start(new Heartbeat("testApp", 1));
            Thread.Sleep(5);
            _sut.Stop();
            Thread.Sleep(2000);
            Assert.Equal(1, client.Messages.Count);
        }

        public void Dispose()
        {
            _sut?.Stop();
            _sut = null;

        }
    }
}
