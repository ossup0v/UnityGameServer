namespace Refactor
{
    public abstract class ClientPacketReceiverMainThreadBase<T> : PacketReceiverMainThreadBase<T> where T : ReadPacketBase
    {
        protected INetworkClientPacketsSender _networkClientPacketsSender;

        protected ClientPacketReceiverMainThreadBase(INetworkClientPacketsSender networkClientPacketsSender, IPacketHandlersHolder packetHandlersHolder) : base(packetHandlersHolder)
        {
            _networkClientPacketsSender = networkClientPacketsSender;
        }
    }
}