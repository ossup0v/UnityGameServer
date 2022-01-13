using System.Net;
using System.Net.Sockets;

namespace Refactor
{
    public struct SocketData
    {
        public TcpClient TcpClient { get; private set; }
        public IPEndPoint IPEndPoint { get; private set; }
        public bool IsTcp { get; private set; }

        public SocketData(TcpClient tcpClient, IPEndPoint iPEndPoint, bool isTcp)
        {
            TcpClient = tcpClient;
            IPEndPoint = iPEndPoint;
            IsTcp = isTcp;
        }
    }
}