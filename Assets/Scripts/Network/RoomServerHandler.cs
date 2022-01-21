using System;
using System.Linq;
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

    public static void PlayersData(Packet packet)
    {
        Debug.Log("Receive players data");

        var amountOfUsers = packet.ReadInt();
        var data = new PlayerMetagameData[amountOfUsers];

        for (int i = 0; i < amountOfUsers; i++)
        {
            data[i] = new PlayerMetagameData
            { 
                Id = packet.ReadGuid(),
                Team = packet.ReadInt(),
                Username = packet.ReadString(),
            };
         
            PlayersManager.AddPlayerData(data[i]);
        }

        Debug.Log(string.Join(";", data.Select(x => x.ToString())));

        RoomSendServer.GameRoomReadyToConnectPlayers();
    }
}