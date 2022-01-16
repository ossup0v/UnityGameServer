using Refactor;

public interface IPacketHandleable
{
    int PacketID { get; }
    void HandleBytes(ref SocketData socketData, byte[] packetBytes, int readOffset);
}