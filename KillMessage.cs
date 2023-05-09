using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
    //A thread that receives this message should exit
    class KillMessage : Message
    {
        public KillMessage()
            : base(-1, -1)
        {
        }
    }
}
