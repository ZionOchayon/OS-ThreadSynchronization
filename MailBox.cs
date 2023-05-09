using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadSynchronization
{
    //DO NOT CHANGE THE CODE IN THIS CLASS (except for testing purposes)

    //The mailbox implementation here does not avoid races. Also, messages may be lost if the mailbox is full.
    class MailBox
    {
        private Message[] m_aMessageBuffer;
        private int m_cMessages;
        private int m_cMaxMessages;
        
        //max messages is an upper bound on the number of messages in the mail box
        public MailBox(int cMaxMessages)
        {
            m_cMaxMessages = cMaxMessages;
            m_aMessageBuffer = new Message[cMaxMessages];
            m_cMessages = 0;
        }
        public MailBox()
            : this(1000)
        {
        }

        //writes a message to the mailbox
        public virtual void Write(Message msg)
        {
            if (m_cMessages < m_cMaxMessages)
            {
                m_aMessageBuffer[m_cMessages] = msg;
                m_cMessages++;
            }
        }

        //reads a message from the mailbox
        public virtual Message Read()
        {
            Message msg = null;
            if (m_cMessages > 0)
            {
                m_cMessages--;
                Thread.Sleep(10);
                msg = m_aMessageBuffer[m_cMessages];
            }
            return msg;
        }

        //checks if a mailbox is empty
        public bool IsEmpty()
        {
            return m_cMessages == 0;
        }
    }
}
