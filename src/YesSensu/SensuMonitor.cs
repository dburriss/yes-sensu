using System;
using YesSensu.Core;
using YesSensu.Messages;

namespace YesSensu
{
    /// <summary>
    /// I am a monitor updater for your application
    /// </summary>
    public class SensuMonitor : IDisposable
    {
        private readonly ISensuClient _sensuClient;
        private readonly string _appName;

        /// <summary>
        /// Constructor for SensuMonitor
        /// </summary>
        /// <param name="sensuClient">The client used to send messages to Sensu</param>
        /// <see cref="SensuClientBase"/>
        /// <seealso cref="SensuUdpClient"/>
        /// <seealso cref="SensuTcpClient"/>
        /// <param name="appName">The name of the application being monitored</param>
        public SensuMonitor(ISensuClient sensuClient, string appName)
        {
            _sensuClient = sensuClient;
            _appName = appName;
            _sensuClient.Connect();
        }

        /// <summary>
        /// I send a hearbeat manually from your application
        /// </summary>
        /// <remarks>Heartbeats are used to indicate your application as a whole is Ok.</remarks>
        /// <param name="period">Time in seconds between </param>
        /// <param name="message">Message to send with update (optional)</param>
        public virtual void Heartbeat(int period = 60, string message = "")
        {
            _sensuClient.Send(
                new Heartbeat(_appName, period)
                {
                    Output = message
                }
            );
        }

        /// <summary>
        /// I send an Ok message from your application
        /// </summary>
        /// <param name="name">Name of the subsystem that is Ok</param>
        /// <param name="message">Message to send with update (optional)</param>
        public virtual void Ok(string name, string message = "")
        {
            _sensuClient.Send(
                new Ok(_appName, name)
                {
                    Output = message
                }
            );
        }

        /// <summary>
        /// I send a Warning from your application
        /// </summary>
        /// <param name="name">Name of the subsystem that caused a warning</param>
        /// <param name="message">Message to send with update (optional)</param>
        public virtual void Warning(string name, string message = "")
        {
            _sensuClient.Send(
                new Warning(_appName, name)
                {
                    Output = message
                }
            );
        }

        /// <summary>
        /// I send an Error from your application
        /// </summary>
        /// <param name="name">Name of the subsystem that caused a error</param>
        /// <param name="message">Message to send with update (optional)</param>
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
