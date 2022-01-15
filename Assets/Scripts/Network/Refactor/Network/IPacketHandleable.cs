using Refactor;

public interface IPacketHandleable
{
    void HandleBytes(ref SocketData socketData, byte[] packetBytes, int readOffset);
}