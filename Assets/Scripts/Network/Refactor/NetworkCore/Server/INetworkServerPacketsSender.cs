using System.Net;
using System.Net.Sockets;

namespace Refactor
{
    public interface INetworkServerPacketsSender
    {
        void SendTCP(TcpClient tcpClient, WritePacketBase writePacket);
        void SendUDP(IPEndPoint IPEndPoint, WritePacketBase writePacket);
    }
}
