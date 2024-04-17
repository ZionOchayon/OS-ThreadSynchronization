using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
    class RoutingMessage : Message
    {
        private Dictionary<int, int> m_dNodeDistances;
        public IEnumerable<int> NodeIDs { get { return m_dNodeDistances.Keys; } }
        public Dictionary<int, int> Distances { get { return m_dNodeDistances; } }    

        
        public RoutingMessage(int iSender, int iTarget, Dictionary<int, int> dNodeDistances)
            : base(iSender, iTarget)
        {
            m_dNodeDistances = new Dictionary<int,int>( dNodeDistances);
        }


        //gets the list of nodes in the routing table
        public List<int> GetAllNodes()
        {
            return new List<int>(m_dNodeDistances.Keys);

        }
        //returns the distance specified in the table to a specific node 
        public int GetDistance(int iNode)
        {
            return m_dNodeDistances[iNode];
        }
    }
}
