using System;

namespace YesSensu.Core.Messages
{
    public abstract class SensuBase
    {
        public string Name { get; private set; }
        public string Output { get; set; }
        public Status Status { get; private set; }

        
        protected SensuBase(string name, Status status)
        {
            if(string.IsNullOrEmpty(name) || name == "_heartbeat")
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Status = status;
            Output = "";
        }

    }
}