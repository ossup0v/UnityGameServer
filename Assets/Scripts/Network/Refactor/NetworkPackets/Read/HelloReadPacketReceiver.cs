using UnityEngine;

namespace Refactor
{
    // [InitPacketReceiver(typeof(ServerNetworkBytesReader))]
    public sealed class HelloReadPacketReceiver : ServerPacketReceiverMainThreadBase<HelloReadPacket>
    {
        protected override int _packetID => HelloReadPacket.PacketID_1;

        public HelloReadPacketReceiver(IClientsHolder clientsHolder, INetworkServerPacketsSender networkServerPacketsSender, IPacketHandlersHolder packetHandlersHolder) : base(clientsHolder, networkServerPacketsSender, packetHandlersHolder)
        {
        }

        protected override void ReceivePacketMainThread(HelloReadPacket packet)
        {
            Debug.Log("got something");
            var helloResponsePacket = new HelloResponseWritePacket();
            var someGUID = System.Guid.NewGuid();
            helloResponsePacket.ClientID = someGUID;
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
}