public interface IPacketReceivable<T> where T : ReadPacketBase
{
    void ReceivePacket(T packet);
}