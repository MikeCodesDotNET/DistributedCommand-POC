using System;
using System.Collections.Generic;
using System.Text;

namespace Ongar.Messaging.Messages
{
    public class ClusterInformationResponseMessage
    {
        public Dictionary<Guid, string> ExistingConnections { get; set; }

        public ClusterInformationResponseMessage()
        {
            ExistingConnections = new Dictionary<Guid, string>();
        }
    }
}
