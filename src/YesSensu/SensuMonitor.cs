using System;
using YesSensu.Core;
using YesSensu.Messages;

namespace YesSensu
{
    public class SensuMonitor : IDisposable
    {
        private readonly ISensuClient _sensuClient;
        private readonly string _appName;

        public SensuMonitor(ISensuClient sensuClient, string appName)
        {
            _sensuClient = sensuClient;
            _appName = appName;
            _sensuClient.Connect();
        }

        public virtual void Heartbeat(int period = 60, string message = "")
        {
            _sensuClient.Send(
                new Heartbeat(_appName, period)
                {
                    Output = message
                }
            );
        }

        public virtual void Ok(string name, string message = "")
        {
            _sensuClient.Send(
                new Ok(_appName, name)
                {
                    Output = message
                }
            );
        }

        public virtual void Warning(string name, string message = "")
        {
            _sensuClient.Send(
                new Warning(_appName, name)
                {
                    Output = message
                }
            );
        }

        public virtual void Error(string name, string message = "")
        {
            _sensuClient.Send(
                new Error(_appName, name)
                {
                    Output = message
                }
            );
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                _sensuClient?.Dispose();
            }
        }
    }
}
