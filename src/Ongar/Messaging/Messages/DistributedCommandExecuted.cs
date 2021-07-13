using System;
using System.Collections.Generic;
using System.Text;

namespace Ongar.Messaging.Messages
{
    public class DistributedCommandExecuted
    {
        public object Sender { get; set; }

        public string TargetCommand { get; set; }

        public DateTime ExecutedAt { get; set; }
    }
}
