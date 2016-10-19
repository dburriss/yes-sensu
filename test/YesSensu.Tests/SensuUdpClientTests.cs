using System;
using System.Net.Sockets;
using Xunit;
using YesSensu.Messages;

namespace YesSensu.Tests
{
    public class SensuUdpClientTests
    {
        [Fact]
        public void Send_WithUnavailableServerAndThrowOnErrorFalse_DoesNotThrowError()
        {
            var hasError = false;
            var sut = new SensuUdpClient("iamaghost", 0000, false);
            sut.Connect();

            try
            {
                sut.Send(new Ok("MyApp", "SomeMetric"));
            }
            catch (Exception)
            {
                hasError = true;
            }

            Assert.False(hasError);
        }

        [Fact]
        public void Send_WithUnavailableServerAndThrowOnErrorTrue_ThrowsError()
        {
            var sut = new SensuUdpClient("iamaghost", 0000, true);
            sut.Connect();
            Assert.Throws<SocketException>(() => sut.Send(new Ok("MyApp", "SomeMetric")));
            
        }
    }
}
