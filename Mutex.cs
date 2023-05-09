using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSynchronization
{
    public class Mutex
    {
        private System.Threading.Mutex m_mMutex;
        public Mutex()
        {
            m_mMutex = new System.Threading.Mutex();
        }
        public void Lock()
        {
            m_mMutex.WaitOne();
        }
        public void Unlock()
        {
            m_mMutex.ReleaseMutex();
        }
    }
}
