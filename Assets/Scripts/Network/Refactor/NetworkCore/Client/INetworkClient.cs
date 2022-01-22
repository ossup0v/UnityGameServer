using System;

namespace Refactor
{
    public interface INetworkClient
    {
        INetworkClientPacketsSender NetworkClientPacketsSender { get; }
        IPacketHandlersHolder PacketHandlersHolder { get; }
        int BufferSize { get; }

        void Connect(string ip, int port, Action connectedToUDPServer, Action connectedToTcpServer);
        void CloseConnection();
    }
}