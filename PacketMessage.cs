using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
    //this message encapulates a part of a string message sent from one node (sender) to another node (target)
    //the string is broken into "packets" of size 1 char
    //in addition the location of the char, the ID of the complete message, and the size of the complete message are sent alongside the packet (char) data
    class PacketMessage : Message
    {
        public int MessageID { get; private set; }
        public int Size { get; private set; }
        public char Packet { get; private set; }
        public int Location { get; private set; }
        public PacketMessage(int iSender, int iTarget, int iMessageID, char chPacket, int iLocation, int iSize)
            : base(iSender, iTarget)
        {
            Size = iSize;
            Packet = chPacket;
            Location = iLocation;
            MessageID = iMessageID;
        }
    }
}
