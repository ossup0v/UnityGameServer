using UnityEngine;

public class RoomServerHandler
{
    public static void GameRoomData(Packet packet)
    {
        var roomId = packet.ReadGuid();

        Room.RoomId = roomId;

        Debug.Log("Receive game room id");

        RoomSendServer.RoomIsLaunched();
    }
}