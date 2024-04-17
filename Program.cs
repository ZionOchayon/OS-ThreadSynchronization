using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace ThreadSynchronization
{
    class Program
    {
        static void TestRouting(Node[] aNodes)
        {
            int iNode = 0, cNodes = aNodes.Length;
            Thread[] aThreads = new Thread[cNodes];
            for (iNode = 0; iNode < cNodes; iNode++)
            {
                aThreads[iNode] = aNodes[iNode].Start();
            }

            Thread.Sleep(10000);

            foreach (Node n in aNodes)
            {
                n.GetMailBox().Send(new KillMessage());
                Thread.Sleep(100);
            }

            while (aThreads[0].IsAlive) ;
        }

        static void TestMessagePassing(Node[] aNodes, int cMessages)
        {
            int iNode = 0, iOtherNode = 0, cNodes = aNodes.Length;
            Thread[] aThreads = new Thread[cNodes];
            Random rnd = new Random();

            for (iNode = 0; iNode < cNodes; iNode++)
            {
                aThreads[iNode] = aNodes[iNode].Start();
            }

            Thread.Sleep(10000);

            Debug.WriteLine("Started sending messages");
            int iMessage = 0;
            while (iMessage < cMessages)
            {
                iNode = rnd.Next(cNodes);
                iOtherNode = rnd.Next(cNodes);
                if (aNodes[iNode].SendMessage("Message " + iMessage + " from " + iNode + " to " + iOtherNode, iMessage, iOtherNode)) 
                    iMessage++;
            }
            Debug.WriteLine("All messages sent!");

            Thread.Sleep(10000);
            
            foreach (Node n in aNodes)
            {
                n.GetMailBox().Send(new KillMessage());
                Thread.Sleep(100);
            }

            while (aThreads[0].IsAlive) ;
        }

        static Node[] CreateRandomGraph(int cNodes, double dDensity)
        {
            int iNode = 0, iOtherNode = 0;
            Node[] aNodes = new Node[cNodes];
            Random rnd = new Random(0);
            for (iNode = 0; iNode < cNodes; iNode++)
            {
                aNodes[iNode] = new Node(iNode);
            }
            for (iNode = 0; iNode < cNodes - 1; iNode++)
            {

                for (iOtherNode = iNode + 1; iOtherNode < cNodes; iOtherNode++)
                {
                    if (rnd.NextDouble() < dDensity)
                    {
                        Node.SetLink(aNodes[iNode], aNodes[iOtherNode]);
                    }
                }
            }
            return aNodes;
        }

        static Node[] CreateBinaryTree(int cNodes)
        {
            int iNode = 0;
            Node[] aNodes = new Node[cNodes];
            Random rnd = new Random(0);
            for (iNode = 0; iNode < cNodes; iNode++)
            {
                aNodes[iNode] = new Node(iNode);
            }
            for (iNode = 2; iNode <= cNodes; iNode++)
            {
                Node.SetLink(aNodes[iNode - 1], aNodes[iNode / 2 - 1]);
            }
            return aNodes;
        }

        static void Main(string[] args)
        {
            FileStream fs = new FileStream("Debug.txt", FileMode.Create);
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.Listeners.Add(new TextWriterTraceListener(fs));
            try
            {
                int cNodes = 20;
                double dDensity = 0.2;
                Node[] aNodes = CreateRandomGraph(cNodes, dDensity);
                //Node[] aNodes = CreateBinaryTree(cNodes);
                TestMessagePassing(aNodes, 100);
                //TestRouting(aNodes);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            Debug.Flush();
            Debug.Close();
            fs.Close();

        }
    }
}
