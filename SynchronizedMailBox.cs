using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ThreadSynchronization
{
    class SynchronizedMailBox : MailBox
    {
        
        private Mutex mutex;
        private Semaphore semaphore;

        public SynchronizedMailBox(int cMaxMessages) : base(cMaxMessages) 
        {
            mutex = new Mutex();
            semaphore = new Semaphore(0);

        }
        public SynchronizedMailBox() : this(1000)
        {
        }

        //writes a message to the mailbox
        public override void Write(Message msg)
        {
            mutex.Lock();
            base.Write(msg);
            mutex.Unlock();
            semaphore.Up();
        }

        //reads a message from the mailbox
        public override Message Read()
        {
            semaphore.Down();
            mutex.Lock();
            Message msg = base.Read();
            mutex.Unlock();
            return msg;
        }
    }
}
