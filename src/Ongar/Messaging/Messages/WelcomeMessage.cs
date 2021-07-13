using System;

namespace Ongar.Messaging.Messages
{
    internal class WelcomeMessage
    {
        public Guid Id { get; set; }

        public string IPAddress { get; set; }

        public int PortNumber { get; set; }
    }
}
