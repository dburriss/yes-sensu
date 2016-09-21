using System;

namespace YesSensu.Core
{
    public interface ISensuClient : IDisposable
    {
        void Connect();
        void Send<TMessage>(TMessage message);
    }
}