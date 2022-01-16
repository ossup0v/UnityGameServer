using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public class NetworkServerIniter : MonoBehaviour
    {
        private UDPServer _udpServer;
        private TCPServer _tcpServer;

        private ServerNetworkBytesReader _serverNetworkPacketsReceiver;
        private NetworkServerPacketsSender _networkServerPacketsSender;
        private HelloPacketReceiver _helloPacketReceiver;

        public int BufferSize { get; } = 1024;

        private void Awake()
        {
            _serverNetworkPacketsReceiver = new ServerNetworkBytesReader();
            _udpServer = new UDPServer(BufferSize, _serverNetworkPacketsReceiver);
            _tcpServer = new TCPServer(BufferSize, _serverNetworkPacketsReceiver);
            _networkServerPacketsSender = new NetworkServerPacketsSender(BufferSize, _udpServer, _tcpServer);
            TestBind();
        }

        private void TestBind()
        {
            var port = 29000;
            _udpServer.Bind(port);
            _tcpServer.Bind(port);

            _helloPacketReceiver = new HelloPacketReceiver(_serverNetworkPacketsReceiver, _networkServerPacketsSender);
        }

        private void OnDestroy()
        {
            _serverNetworkPacketsReceiver.Dispose();
            _helloPacketReceiver.Dispose();
        }
    }
}