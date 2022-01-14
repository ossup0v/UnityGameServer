namespace Refactor
{
    public ref struct PacketHeader
    {
        public int PacketID { get; private set; }
        
        public PacketHeader(int packetID)
        {
            PacketID = packetID;
        }
    }
}
