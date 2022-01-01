using System;

public class RoomSendServer
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Room.Server.Tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Room.Server.Udp.SendData(packet);
    }

    public static void RoomIsLaunched(Guid roomId, Guid creatorId, string mode, string title, int maxPlayerCount, int port)
    {
        using (Packet packet = new Packet((int)ToServerFromGameRoom.gameRoomLaunched))
        {
            packet.Write(roomId);
            packet.Write(creatorId);
            packet.Write(mode); 
            packet.Write(title);
            packet.Write(maxPlayerCount);
            packet.Write(port);

            SendTCPData(packet);
        }
    }
}
