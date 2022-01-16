using UnityEngine;

namespace Refactor
{
    // [InitPacketReceiver(typeof(ServerNetworkBytesReader))]
    public sealed class HelloPacketReceiver : PacketReceiverBase<HelloReadPacket>
    {
        protected override int _packetID => HelloReadPacket.PacketID_1;
        private NetworkServerPacketsSender _networkServerPacketsSender;

        public HelloPacketReceiver(IPacketHandlersHolder packetHandlersHolder, NetworkServerPacketsSender networkServerPacketsSender) : base(packetHandlersHolder)
        {
            _networkServerPacketsSender = networkServerPacketsSender;
        }

        public override void ReceivePacket(HelloReadPacket packet)
        {
            Debug.Log("got something");
            var helloResponsePacket = new HelloResponseWritePacket();
            var someGUID = System.Guid.NewGuid();
            helloResponsePacket.GUID = someGUID;
            if (packet.SocketData.IsTcp)
            {
                // _networkServerPacketsSender.SendTCP(packet.SocketData.TcpClient, helloResponsePacket);
            }
            else
            {
                Debug.Log("it is not TCP connection");
            }
        }
    }

    public sealed class HelloResponseWritePacket : WritePacketBase
    {
        public override int PacketID => 1;

        public override void SerializePacket()
        {
            
        }
    }
}