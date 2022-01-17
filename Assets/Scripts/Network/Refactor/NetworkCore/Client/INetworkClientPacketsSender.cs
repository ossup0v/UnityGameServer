using System;

namespace Refactor
{
    public interface INetworkClientPacketsSender
    {
        void SendTCP(WritePacketBase writePacket);
        void SendUDP(WritePacketBase writePacket);
        void SetClientID(Guid clientID);
    }
}
