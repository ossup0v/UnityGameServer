using System;
using System.Net;
using System.Net.Sockets;

namespace Refactor
{
    public interface IClientsHolder
    {
        IPEndPoint GetIPEndPoint(Guid clientID);
        TcpClient GetTcpClient(Guid clientID);
        void AddRemoteIPEndPoint(Guid clientID, IPEndPoint ipEndPoint);
        void AddRemoteTcpClient(Guid clientID, TcpClient tcpClient);
        void RemoveRemoteIPEndPoint(Guid clientID);
        void RemoveRemoteTcpClient(Guid clientID);
    }
}