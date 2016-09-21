using System;
using YesSensu.Core.Messages;

namespace YesSensu.Core
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

        public virtual void Hearbeat(int period = 60, string message = "")
        {
            _sensuClient.Send(
                new Heartbeat(_appName, period)
                {
                    Output = message
                }
            );
        }

        public virtual void Ok(string message = "")
        {
            _sensuClient.Send(
                new AppOk(_appName)
                {
                    Output = message
                }
            );
        }

        public virtual void Warning(string message = "")
        {
            _sensuClient.Send(
                new AppWarning(_appName)
                {
                    Output = message
                }
            );
        }

        public virtual void Error(string message = "")
        {
            _sensuClient.Send(
                new AppError(_appName)
                {
                    Output = message
                }
            );
        }

        public virtual void Metric(string key, Status status, string message = "")
        {
            _sensuClient.Send(
                new AppUpdate(_appName, key, status)
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
