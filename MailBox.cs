using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace ThreadSynchronization
{
    class MailBox
    {
        Queue<Message> m_aMessageBuffer;
        private int m_cMaxMessages;
        
        //max messages is an upper bound on the number of messages in the mail box
        public MailBox(int cMaxMessages)
        {
            m_cMaxMessages = cMaxMessages;
            m_aMessageBuffer = new Queue<Message>(cMaxMessages);
        }
        public MailBox()
            : this(1000)
        {
        }

        //writes a message to the mailbox
        public virtual void Write(Message msg)
        {
            if (m_aMessageBuffer.Count < m_cMaxMessages)
            {
                m_aMessageBuffer.Enqueue(msg);
            }
        }

        //reads a message from the mailbox
        public virtual Message Read()
        {
            if (m_aMessageBuffer.Count > 0)
            {
                Thread.Sleep(10);
                return m_aMessageBuffer.Dequeue();
            }
            return null;
        }

        //checks if a mailbox is empty
        public bool IsEmpty()
        {
            return m_aMessageBuffer.Count == 0;
        }
    }
}
