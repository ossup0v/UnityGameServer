namespace Refactor
{
    public abstract class ServerPacketReceiverBase<T> : PacketReceiverBase<T> where T : ReadPacketBase
    {
        protected INetworkServerPacketsSender _networkServerPacketsSender;
        protected IClientsHolder _clientsHolder;

        protected ServerPacketReceiverBase(IClientsHolder clientsHolder, INetworkServerPacketsSender networkServerPacketsSender, IPacketHandlersHolder packetHandlersHolder) : base(packetHandlersHolder)
        {
            _networkServerPacketsSender = networkServerPacketsSender;
            _clientsHolder = clientsHolder;
        }
    }
}