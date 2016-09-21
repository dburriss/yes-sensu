using System.Collections.Generic;
using System.Linq;
using Xunit;
using YesSensu.Core.Messages;

namespace YesSensu.Core.Tests
{
    public class SensuMonitorTests
    {
        [Fact]
        public void Heartbeat_WithNoParameters_SendsHeartbeat()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Hearbeat();
            var message = client.Messages.First();
            Assert.IsType<Heartbeat>(message);
        }

        [Fact]
        public void Heartbeat_WithOutput_SetsOutputOnHeartbeat()
        {
            var text = "text";
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Hearbeat(message: text);
            var message = client.Messages.First() as Heartbeat;
            Assert.Equal(text, message.Output);
        }

        [Fact]
        public void Heartbeat_WithNoParameters_SetsTtlTo60()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Hearbeat();
            var message = client.Messages.First() as Heartbeat;
            Assert.Equal(60, message.Ttl);
        }

        [Fact]
        public void Heartbeat_WithPeriod5_SetsTtlTo5()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Hearbeat(period:5);
            var message = client.Messages.First() as Heartbeat;
            Assert.Equal(5, message.Ttl);
        }

        [Fact]
        public void Ok_WithNoParameters_SendsAppOk()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Ok();
            var message = client.Messages.First();
            Assert.IsType<AppOk>(message);
        }

        [Fact]
        public void Warning_WithNoParameters_SendsAppWarning()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Warning();
            var message = client.Messages.First();
            Assert.IsType<AppWarning>(message);
        }

        [Fact]
        public void Error_WithNoParameters_SendsAppError()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Error();
            var message = client.Messages.First();
            Assert.IsType<AppError>(message);
        }

        [Fact]
        public void Metric_WithAnyParameters_SendsAppUpdate()
        {
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Metric("some_dependency", Status.Ok);
            sut.Metric("some_dependency", Status.Warning);
            sut.Metric("some_dependency", Status.Error);
            var messages = client.Messages.ToArray();
            Assert.IsType<AppUpdate>(messages[0]);
            Assert.IsType<AppUpdate>(messages[1]);
            Assert.IsType<AppUpdate>(messages[2]);
        }

        [Fact]
        public void Metric_WithKeyX_MessageNameIsX()
        {
            var key = "X";
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "testApp");
            sut.Metric(key, Status.Warning);
            var message = client.Messages.First() as AppUpdate;
            Assert.Equal(key, message.Name);
        }

        [Fact]
        public void Metric_WithAppNameY_MessageSourceIsY()
        {
            var key = "X";
            var client = new MockSensuClient();
            var sut = new SensuMonitor(client, "Y");
            sut.Metric(key, Status.Warning);
            var message = client.Messages.First() as AppUpdate;
            Assert.Equal("Y", message.Source);
        }
    }

    public class MockSensuClient : ISensuClient
    {
        public bool IsDisposed { get; private set; }
        public bool IsConnected { get; private set; }
        public ICollection<object> Messages { get; private set; }

        public MockSensuClient()
        {
            Messages = new List<object>();
        }
        public void Dispose()
        {
            IsDisposed = true;
        }

        public void Connect()
        {
            IsConnected = true;
        }

        public void Send<TMessage>(TMessage message)
        {
            Messages.Add(message);
        }
    }
}
