using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSynchronization
{
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
