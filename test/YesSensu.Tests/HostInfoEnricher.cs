using System;
using System.Net;
using YesSensu.Messages;

namespace YesSensu.Tests
{
    public class HostInfoEnricher : ISensuEnricher
    {
        public void Enrich(IHaveMeta obj)
        {
            AddHostName(obj);
            AddMachineName(obj);
        }

        private void AddMachineName(IHaveMeta obj)
        {
            var machineName = Environment.MachineName;
            obj.AddMeta("machine_name", machineName);
        }

        private static void AddHostName(IHaveMeta obj)
        {
            var hostName = Dns.GetHostName();
            obj.AddMeta("host_name", hostName);
        }
    }
}