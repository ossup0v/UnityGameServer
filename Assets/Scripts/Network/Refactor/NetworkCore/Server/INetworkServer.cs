namespace Refactor
{
    public interface INetworkServer
    {
        INetworkServerPacketsSender NetworkServerPacketsSender { get; }
        IPacketHandlersHolder PacketHandlersHolder { get; }
        IClientsHolder ClientsHolder { get; }
        int BufferSize { get; }

        void Start(int port);
        void Stop();
    }
}
