using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Refactor
{
    public class ClientsHolder
    {
        private Dictionary<Guid, IPEndPoint> _remoteIPEndPointByGuid = new Dictionary<Guid, IPEndPoint>();
        private Dictionary<Guid, TcpClient> _remoteTcpClientByGuid = new Dictionary<Guid, TcpClient>();

        public IPEndPoint GetIPEndPoint(Guid clientGuid)
        {
            if (_remoteIPEndPointByGuid.ContainsKey(clientGuid))
            {
                return _remoteIPEndPointByGuid[clientGuid];
            }
            Logger.WriteError(nameof(ClientsHolder), $"Can't find IpEndPoint with {clientGuid} id");
            return null;
        }

        public TcpClient GetTcpClient(Guid clientGuid)
        {
            if (_remoteTcpClientByGuid.ContainsKey(clientGuid))
            {
                return _remoteTcpClientByGuid[clientGuid];
            }
            Logger.WriteError(nameof(ClientsHolder), $"Can't find TcpClient with {clientGuid} id");
            return null;
        }

        public void AddRemoteIPEndPoint(Guid clientGuid, IPEndPoint ipEndPoint)
        {
            if (_remoteIPEndPointByGuid.ContainsKey(clientGuid) == false)
            {
                _remoteIPEndPointByGuid.Add(clientGuid, ipEndPoint);
            }
        }

        public void AddRemoteTcpClient(Guid clientGuid, TcpClient tcpClient)
        {
            if (_remoteTcpClientByGuid.ContainsKey(clientGuid) == false)
            {
                _remoteTcpClientByGuid.Add(clientGuid, tcpClient);
            }
        }

        public void RemoveRemoteIPEndPoint(Guid clientGuid)
        {
            if (_remoteIPEndPointByGuid.ContainsKey(clientGuid))
            {
                _remoteIPEndPointByGuid.Remove(clientGuid);
            }
        }

        public void RemoveRemoteTcpClient(Guid clientGuid)
        {
            if (_remoteTcpClientByGuid.ContainsKey(clientGuid))
            {
                _remoteTcpClientByGuid.Remove(clientGuid);
            }
        }


    }
}