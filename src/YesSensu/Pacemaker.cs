using System;
using System.Threading;
using YesSensu.Messages;

namespace YesSensu
{
    public class Pacemaker
    {
        public ISensuClient Client { get; }
        private Thread _workerThread;
        private readonly CancellationTokenSource _tokenSource;

        public Pacemaker(ISensuClient client)
        {
            if(client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
            _tokenSource = new CancellationTokenSource();
        }

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

        public void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}