namespace Refactor
{
    public abstract class ClientPacketReceiverBase<T> : PacketReceiverBase<T> where T : ReadPacketBase
    {
        protected INetworkClientPacketsSender _networkClientPacketsSender;

        protected ClientPacketReceiverBase(INetworkClientPacketsSender networkClientPacketsSender, IPacketHandlersHolder packetHandlersHolder) : base(packetHandlersHolder)
        {
            _networkClientPacketsSender = networkClientPacketsSender;
        }
    }
}