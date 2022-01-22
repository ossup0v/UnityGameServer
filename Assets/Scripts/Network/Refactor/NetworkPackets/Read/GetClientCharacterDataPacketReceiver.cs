using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public sealed class GetClientCharacterDataPacketReceiver : ServerPacketReceiverMainThreadBase<GetClientCharacterDataReadPacket>
    {
        protected override int _packetID => GetClientCharacterDataReadPacket.PacketID_2;

        public event Action<Guid> ReceivedGetClientCharacterPacket;

        public GetClientCharacterDataPacketReceiver(IClientsHolder clientsHolder, INetworkServerPacketsSender networkServerPacketsSender, IPacketHandlersHolder packetHandlersHolder) : base(clientsHolder, networkServerPacketsSender, packetHandlersHolder)
        {
        }

        protected override void ReceivePacketMainThread(GetClientCharacterDataReadPacket packet)
        {
            // TODO: переделать
            var clientsHolderContainsClientID = _clientsHolder.ContainsClient(packet.ClientID);
            if (clientsHolderContainsClientID)
            {
                ReceivedGetClientCharacterPacket?.Invoke(packet.ClientID);
            }
            Debug.Log(packet.ClientID + " packet GetClientCharacterDataReadPacket " + clientsHolderContainsClientID);
        }
    }
}