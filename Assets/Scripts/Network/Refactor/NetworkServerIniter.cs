using System.Collections;
using UnityEngine;

namespace Refactor
{
    public class NetworkServerIniter : MonoBehaviour
    {
        private UDPServer _udpServer;
        private TCPServer _tcpServer;

        private ServerNetworkBytesReader _serverNetworkPacketsReceiver;
        private NetworkServerPacketsSender _networkServerPacketsSender;
        private HelloReadPacketReceiver _helloPacketReceiver;
        private ClientsHolder _clientsHolder;

        public int BufferSize { get; } = 1024;

        private void Awake()
        {
            _serverNetworkPacketsReceiver = new ServerNetworkBytesReader();
            _udpServer = new UDPServer(BufferSize, _serverNetworkPacketsReceiver);
            _tcpServer = new TCPServer(BufferSize, _serverNetworkPacketsReceiver);
            _networkServerPacketsSender = new NetworkServerPacketsSender(BufferSize, _udpServer, _tcpServer);
            _clientsHolder = new ClientsHolder();
            TestBind();
        }

        private void TestBind()
        {
            var port = 29000;
            _udpServer.Bind(port);
            _tcpServer.Bind(port);

            _helloPacketReceiver = new HelloReadPacketReceiver(_clientsHolder, _networkServerPacketsSender, _serverNetworkPacketsReceiver);
        }

        private void OnDestroy()
        {
            _serverNetworkPacketsReceiver.Dispose();
            _helloPacketReceiver.Dispose();
        }
    }
}