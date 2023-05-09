using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
    //This class provides write only access to a mailbox, used by neighbors to send messages to a node
    class MailBoxWriter
    {
        private MailBox m_mbMailBox;

        public MailBoxWriter(MailBox mb)
        {
            m_mbMailBox = mb;
        }

        public void Send(Message msg)
        {
            m_mbMailBox.Write(msg);
        }
    }
}
