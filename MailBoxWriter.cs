using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
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
