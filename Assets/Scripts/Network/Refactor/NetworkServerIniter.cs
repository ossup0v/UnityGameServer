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

        public int BufferSize { get; } = 1024;

        private void Awake()
        {
            _serverNetworkPacketsReceiver = new ServerNetworkBytesReader();
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

        private void OnDestroy()
        {
            _serverNetworkPacketsReceiver.Dispose();
        }
    }

    [InitPacketReceiverAttribute(typeof(ServerNetworkBytesReader))]
    public sealed class HelloPacketReceiver : PacketReceiverBase<HelloReadPacket>
    {
        protected override int _packetID => HelloReadPacket.PacketID_1;

        public HelloPacketReceiver(IPacketHandlersHolder packetHandlersHolder) : base(packetHandlersHolder)
        {
        }

        public override void ReceivePacket(HelloReadPacket packet)
        {
            Debug.Log("packed received");
        }
    }
    [InitPacketReceiverAttribute(typeof(ServerNetworkBytesReader))]
    public sealed class HelloPacketReceiver2 : PacketReceiverBase<HelloReadPacket>
    {
        protected override int _packetID => HelloReadPacket.PacketID_1;

        public HelloPacketReceiver2(IPacketHandlersHolder packetHandlersHolder) : base(packetHandlersHolder)
        {
        }

        public override void ReceivePacket(HelloReadPacket packet)
        {
            Debug.Log("packed received ANOTHER ONE");
        }
    }
}