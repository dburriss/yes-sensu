using System;
using System.Threading;
using YesSensu.Core;
using YesSensu.Messages;

namespace YesSensu
{
    /// <summary>
    /// Pacemaker sends regular heartbeats to Sensu within a Thread it creates
    /// </summary>
    public class Pacemaker
    {
        public ISensuClient Client { get; }
        private Thread _workerThread;
        private readonly CancellationTokenSource _tokenSource;

        /// <summary>
        /// Constructor for Pacemaker
        /// </summary>
        /// <param name="client">The Sensu client used to send the hearbeat</param>
        public Pacemaker(ISensuClient client)
        {
            if(client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
            _tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Start the heartbeat sends
        /// </summary>
        /// <param name="heartBeat">The Heartbeat that is sent. The frequency of sending is extracted from this.</param>
        public void Start(Heartbeat heartBeat)
        {
            if(heartBeat == null)
                throw new ArgumentNullException(nameof(heartBeat));

            Client.Connect();
            _workerThread = new Thread(() =>
            {
                Run(heartBeat, _tokenSource.Token);
            });
            _workerThread.Start();
        }

        private void Run(Heartbeat heartBeat, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    Client.Send(heartBeat);
                    Thread.Sleep(heartBeat.Ttl*1000);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Stops the Pacemaker from sending heartbeats
        /// </summary>
        public void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}