using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Refactor
{
    public class ClientsHolder : IClientsHolder
    {
        private Dictionary<Guid, IPEndPoint> _remoteIPEndPointByClientID = new Dictionary<Guid, IPEndPoint>();
        private Dictionary<Guid, TcpClient> _remoteTcpClientByClientID = new Dictionary<Guid, TcpClient>();

        public IPEndPoint GetIPEndPoint(Guid clientID)
        {
            if (_remoteIPEndPointByClientID.ContainsKey(clientID))
            {
                return _remoteIPEndPointByClientID[clientID];
            }
            Logger.WriteError(nameof(ClientsHolder), $"Can't find IpEndPoint with {clientID} id");
            return null;
        }

        public TcpClient GetTcpClient(Guid clientID)
        {
            if (_remoteTcpClientByClientID.ContainsKey(clientID))
            {
                return _remoteTcpClientByClientID[clientID];
            }
            Logger.WriteError(nameof(ClientsHolder), $"Can't find TcpClient with {clientID} id");
            return null;
        }

        public void AddRemoteIPEndPoint(Guid clientID, IPEndPoint ipEndPoint)
        {
            if (_remoteIPEndPointByClientID.ContainsKey(clientID) == false)
            {
                _remoteIPEndPointByClientID.Add(clientID, ipEndPoint);
            }
        }

        public void AddRemoteTcpClient(Guid clientID, TcpClient tcpClient)
        {
            if (_remoteTcpClientByClientID.ContainsKey(clientID) == false)
            {
                _remoteTcpClientByClientID.Add(clientID, tcpClient);
            }
        }

        public void RemoveRemoteIPEndPoint(Guid clientID)
        {
            if (_remoteIPEndPointByClientID.ContainsKey(clientID))
            {
                _remoteIPEndPointByClientID.Remove(clientID);
            }
        }

        public void RemoveRemoteTcpClient(Guid clientID)
        {
            if (_remoteTcpClientByClientID.ContainsKey(clientID))
            {
                _remoteTcpClientByClientID.Remove(clientID);
            }
        }
    }
}