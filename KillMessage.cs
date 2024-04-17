using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
    class KillMessage : Message
    {
        public KillMessage()
            : base(-1, -1)
        {
        }
    }
}
