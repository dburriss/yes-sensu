using System.Linq;
using Xunit;
using YesSensu.Messages;

namespace YesSensu.Tests
{
    public class SensuMonitorTests
    {
        [Fact]
        public void Heartbeat_WithNoParameters_SendsHeartbeat()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Heartbeat();
            var message = client.Messages.First();
            Assert.IsType<Heartbeat>(message);
        }

        [Fact]
        public void Heartbeat_WithOutput_SetsOutputOnHeartbeat()
        {
            var text = "text";
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Heartbeat(message: text);
            var message = client.Messages.First() as Heartbeat;
            Assert.Equal(text, message.Output);
        }

        [Fact]
        public void Heartbeat_WithNoParameters_SetsTtlTo60()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Heartbeat();
            var message = client.Messages.First() as Heartbeat;
            Assert.Equal(60, message.Ttl);
        }

        [Fact]
        public void Heartbeat_WithPeriod5_SetsTtlTo5()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Heartbeat(period:5);
            var message = client.Messages.First() as Heartbeat;
            Assert.Equal(5, message.Ttl);
        }

        [Fact]
        public void Ok_WithNoParameters_SendsAppOk()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Ok("some_dependency");
            var message = client.Messages.First();
            Assert.IsType<Ok>(message);
        }

        [Fact]
        public void Warning_WithNoParameters_SendsAppWarning()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Warning("some_dependency");
            var message = client.Messages.First();
            Assert.IsType<Warning>(message);
        }

        [Fact]
        public void Error_WithNoParameters_SendsAppError()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Error("some_dependency");
            var message = client.Messages.First();
            Assert.IsType<Error>(message);
        }

    }
}
