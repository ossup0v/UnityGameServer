namespace Refactor
{
    public abstract class ServerPacketReceiverMainThreadBase<T> : PacketReceiverMainThreadBase<T> where T : ReadPacketBase
    {
        protected INetworkServerPacketsSender _networkServerPacketsSender;
        protected IClientsHolder _clientsHolder;

        protected ServerPacketReceiverMainThreadBase(IClientsHolder clientsHolder, INetworkServerPacketsSender networkServerPacketsSender, IPacketHandlersHolder packetHandlersHolder) : base(packetHandlersHolder)
        {
            _networkServerPacketsSender = networkServerPacketsSender;
            _clientsHolder = clientsHolder;
        }
    }
}