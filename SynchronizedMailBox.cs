using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ThreadSynchronization
{
    //This class extends the MailBox, by overriding the read and write methods.
    //The override must call the original implementation, which contain races, protecting the critical sections from races.
    //You cannot define a new message array or any other data structue for messages here.
    //You can, however, add any other fields as you see fit.
    class SynchronizedMailBox : MailBox
    {
        //your code here
        //writes a message to the mailbox
        private Mutex mutex;
        private Semaphore semaphoreFull;

        public SynchronizedMailBox(int cMaxMessages) : base(cMaxMessages) 
        {
            mutex = new Mutex();
            semaphoreFull = new Semaphore(0);

        }
        public SynchronizedMailBox() : this(1000)
        {
        }

        public override void Write(Message msg)
        {
            mutex.Lock();
            base.Write(msg);
            mutex.Unlock();
            semaphoreFull.Up();
        }

        //reads a message from the mailbox
        public override Message Read()
        {
            semaphoreFull.Down();
            mutex.Lock();
            Message msg = base.Read();

            if (msg is KillMessage && !IsEmpty())
            {
                Message tmp = msg;
                msg = base.Read();
                base.Write(tmp);
            }
            else if (msg is KillMessage)
            {
                mutex.Unlock();
                while (Node.reccived < Node.sumMessages)
                {
                    if (!IsEmpty())
                    {
                        break;
                    }
                }
                mutex.Lock();
                if (!IsEmpty())
                {
                    Message tmp = msg;
                    msg = base.Read();
                    base.Write(tmp);
                }
            }
            mutex.Unlock();
            return msg;
        }
    }
}
