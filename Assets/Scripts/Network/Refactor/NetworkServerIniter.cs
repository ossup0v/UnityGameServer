using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public class NetworkServerIniter : MonoBehaviour
    {
        private UDPServer _udpServer;
        private TCPServer _tcpServer;

        private ServerNetworkPacketsReceiver _serverNetworkPacketsReceiver;

        public int BufferSize { get; } = 1024;

        private void Awake()
        {
            _serverNetworkPacketsReceiver = new ServerNetworkPacketsReceiver();
            _udpServer = new UDPServer(BufferSize, _serverNetworkPacketsReceiver);
            _tcpServer = new TCPServer(BufferSize, _serverNetworkPacketsReceiver);            
            TestBind();
        }

        private void TestBind()
        {
            var port = 29000;
            _udpServer.Bind(port);
            _tcpServer.Bind(port);
        }
    }
}