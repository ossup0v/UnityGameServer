using UnityEngine;

namespace Refactor
{
    // [InitPacketReceiver(typeof(ServerNetworkBytesReader))]
    public sealed class HelloPacketReceiver : PacketReceiverMainThreadBase<HelloReadPacket>
    {
        protected override int _packetID => HelloReadPacket.PacketID_1;
        private NetworkServerPacketsSender _networkServerPacketsSender;
        private ClientsHolder _clientsHolder;

        public HelloPacketReceiver(ClientsHolder clientsHolder, IPacketHandlersHolder packetHandlersHolder, NetworkServerPacketsSender networkServerPacketsSender) : base(packetHandlersHolder)
        {
            _networkServerPacketsSender = networkServerPacketsSender;
            _clientsHolder = clientsHolder;
        }

        protected override void ReceivePacketMainThread(HelloReadPacket packet)
        {
            Debug.Log("got something");
            var helloResponsePacket = new HelloResponseWritePacket();
            var someGUID = System.Guid.NewGuid();
            helloResponsePacket.GUID = someGUID;
            if (packet.SocketData.IsTcp)
            {
                _clientsHolder.AddRemoteTcpClient(someGUID, packet.SocketData.TcpClient);
                var remoteTcpClient = _clientsHolder.GetTcpClient(someGUID);
                _networkServerPacketsSender.SendTCP(remoteTcpClient, helloResponsePacket);                
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