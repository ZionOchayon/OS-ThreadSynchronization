using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
    class Message
    {
        public int Sender { get; protected set; }
        public int Target { get; protected set; }
        public Message(int iSender, int iTarget)
        {
            Sender = iSender;
            Target = iTarget;
        }
    }
}
