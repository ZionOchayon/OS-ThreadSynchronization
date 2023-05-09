using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSynchronization
{
    public class Semaphore
    {
        private System.Threading.Semaphore m_sSemaphore;
        public Semaphore(int init)
        {
            m_sSemaphore = new System.Threading.Semaphore(init, int.MaxValue);
        }
        public void Down()
        {
            m_sSemaphore.WaitOne();
        }
        public void Up()
        {
            m_sSemaphore.Release();
        }
    }
}
