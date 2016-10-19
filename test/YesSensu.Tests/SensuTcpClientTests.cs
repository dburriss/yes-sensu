using System;
using Xunit;

namespace YesSensu.Tests
{
    public class SensuTcpClientTests
    {
        //TODO: there are different contraints on Connect and Send across TCP and UDP. This is violation of LSP. Come back and fix this.
        [Fact]
        public void Connect_WithUnavailableServerAndThrowOnErrorFalse_DoesNotThrowError()
        {
            var hasError = false;
            var sut = new SensuTcpClient("iamaghost", 0000, false);
            
            try
            {
                sut.Connect();
            }
            catch (Exception)
            {
                hasError = true;
            }

            Assert.False(hasError);
        }

        [Fact]
        public void Connect_WithUnavailableServerAndThrowOnErrorTrue_ThrowsError()
        {
            var sut = new SensuTcpClient("iamaghost", 0000, true);
            
            Assert.Throws<TimeoutException>(() => sut.Connect());
            
        }
    }
}
