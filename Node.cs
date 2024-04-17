using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace ThreadSynchronization
{ 
    class Node 
    {
        private MailBox m_mbMailBox; //incoming mailbox of the node
        private Dictionary<int, MailBoxWriter> m_dNeighbors; //maps node ids to outgoing mailboxes (the routing table)
        private bool m_bDone; //notifies the thread to terminate
        private Dictionary<int, int> m_dNodeDistances; //routing information 
        private Dictionary<int, int> m_dRoutingInfo; //routing information
        private Dictionary<int, char[]> m_dMessages; //received messages for this node
        private Random m_rndGenerator = new Random();

        public int ID { get; private set; } //the identifier of the node

        public Node(int iID)
        {
            ID = iID;
            m_mbMailBox = new SynchronizedMailBox();
            m_dNeighbors = new Dictionary<int, MailBoxWriter>();
            m_dNodeDistances = new Dictionary<int, int>();
            m_dRoutingInfo = new Dictionary<int, int>();
            m_dMessages = new Dictionary<int, char[]>();
            m_dNodeDistances[ID] = 0;
            m_bDone = false;
        }

        //Returns access to the node's mailbox
        public MailBoxWriter GetMailBox()
        {
            return new MailBoxWriter(m_mbMailBox);
        }

        //sends routing messages to all the immediate neighbors
        private void SendRoutingMessages()
        {
            foreach (KeyValuePair<int, MailBoxWriter> p in m_dNeighbors)
            {
                RoutingMessage rmsg = new RoutingMessage(ID, p.Key, m_dNodeDistances);
                p.Value.Send(rmsg);
            }
        }

        //handles an incoming routing neighbors according to the Bellman-Ford algorithm
        private void HandleRoutingMessage(RoutingMessage rmsg)
        {
            if (UpdateDistances(rmsg))
            {
                SendRoutingMessages();
            }
        }

        //handles an incoming routing neighbors according to the Bellman-Ford algorithm
        private bool UpdateDistances(RoutingMessage rmsg)
        {
            bool bUpdated = false;
            foreach (int iNode in rmsg.NodeIDs)
            {
                int iDistance = rmsg.Distances[iNode];
                if ((!m_dNodeDistances.ContainsKey(iNode)) || (m_dNodeDistances[iNode] > iDistance + 1))
                {
                    m_dNodeDistances[iNode] = iDistance + 1;
                    m_dRoutingInfo[iNode] = rmsg.Sender;
                    bUpdated = true;
                }
            }
            return bUpdated;
        }

        //handles an incoming packet message 
        //the message can be directed to the current node or to another node, in which case it should be forwarded
        private void HandlePacketMessage(PacketMessage pmsg)
        {
            if (pmsg.Target == ID)
            {
                if (!m_dMessages.ContainsKey(pmsg.MessageID))
                {
                    m_dMessages[pmsg.MessageID] = new char[pmsg.Size];
                    for (int i = 0; i < pmsg.Size; i++)
                        m_dMessages[pmsg.MessageID][i] = '\0';
                }
                m_dMessages[pmsg.MessageID][pmsg.Location] = pmsg.Packet;
                bool bDone = true;
                foreach (char c in m_dMessages[pmsg.MessageID])
                {
                    if (c == '\0')
                        bDone = false;
                }
                if (bDone)
                {
                    string sMsg = ID + " received: ";
                    foreach (char c in m_dMessages[pmsg.MessageID])
                        sMsg += c;
                    Debug.WriteLine(sMsg);
                }
            }
            else
            {
                //forward the message to a neighbor using the routing table
                int router = GetRouter(pmsg.Target);
                if (router != -1)
                    m_dNeighbors[router].Send(pmsg);
            }
        }


        //returns the neighboring router for the target node
        private int GetRouter(int iTarget)
        {
            if (!m_dRoutingInfo.ContainsKey(iTarget))
                return -1;
            int iRoutingNeighbor = m_dRoutingInfo[iTarget];
            if (m_rndGenerator.NextDouble() < 0.5)
                iRoutingNeighbor = m_dRoutingInfo.Values.ElementAt(m_rndGenerator.Next(m_dRoutingInfo.Count));
            return iRoutingNeighbor;
        }

        //returns the distance of the routing node
        private int GetDistance(int iTarget)
        {
            return m_dNodeDistances[iTarget];
        }

        //returns the list of all reachable nodes (all the nodes that appear in the routing table)
        private List<int> ReachableNodes()
        {
            return new List<int>(m_dRoutingInfo.Keys);
        }

        //returns the list of recieved messages
        //if a character in a message was not received (the message was not fully received), the array should contain
        //the sepcail character '\0'
        private List<char[]> ReceivedMessages()
        {
            return new List<char[]>(m_dMessages.Values);
        }


        //Node (thread) main method - repeatedly checks for incoming mail and handles it.
        //when the thread is terminated using the KillMessage, outputs the routing table and the list of accepted messages
        public void Run()
        {
            SendRoutingMessages();
            while (!m_bDone)
            {
                Message msg = m_mbMailBox.Read();
                if (msg is RoutingMessage)
                {
                    HandleRoutingMessage((RoutingMessage)msg);                   
                }
                if (msg is PacketMessage)
                {
                    HandlePacketMessage((PacketMessage)msg);
                }
                if (msg is KillMessage)
                    m_bDone = true;
            }
            PrintRoutingTable();
            PrintAllMessages();
        }

        //Creates a thread that executes the Run method, starts it, and returns the created Thread object
        public Thread Start()
        {
            //your code here
            Thread t = new(Run);
            t.Start();
            return t;
        }

        //prints the routing table 
        public void PrintRoutingTable()
        {
            string s = "\nRouting table for " + ID + "\n";
            foreach (int iNode in ReachableNodes())
            {
                s += iNode + ", distance = " + GetDistance(iNode) + ", router = " + GetRouter(iNode) + "\n";
            }
            Debug.WriteLine(s);
        }


        //prints the list of accepted messages
        //if a char is missing, writes '?' instead
        public void PrintAllMessages()
        {
            Debug.WriteLine("Message list of " + ID);
            foreach (char[] aMessage in ReceivedMessages())
            {
                string s = "";
                for (int i = 0; i < aMessage.Length; i++)
                {
                    if (aMessage[i] == '\0')
                        s += "?";
                    else
                        s += aMessage[i];
                }
                Debug.WriteLine(s);
            }
        }


        //Sets a link (immediate access) between two nodes
        public static void SetLink(Node n1, Node n2)
        {
            n1.m_dNeighbors[n2.ID] = n2.GetMailBox();
            n2.m_dNeighbors[n1.ID] = n1.GetMailBox();
        }


        //Send a string message from one machine to another
        public bool SendMessage(string sMessage, int iMessageID, int iTarget)
        {
            int router = GetRouter(iTarget);
            if (router == -1)
                return false;
            int counter = 0;
            foreach (char packet in sMessage)
                m_dNeighbors[router].Send(new PacketMessage(ID, iTarget, iMessageID, packet, counter++, sMessage.Length));
            return true;
        }
    }
}
