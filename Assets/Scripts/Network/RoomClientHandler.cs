using System;
using UnityEngine;

public class RoomClientHandler
{
    public static void WelcomeReceived(Guid fromClient, Packet packet)
    {
        Debug.Log("Welcome received");
#warning todo сделать отправку этой инфы через сервер
        Guid clientIdCheck = packet.ReadGuid();
        string username = packet.ReadString();
        int team = packet.ReadInt();

        Debug.Log($"Welcome received from id on server {fromClient}, in packet {clientIdCheck}, team: {team}");

        Debug.Log($"{Room.Clients[fromClient].tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }

        Room.Clients[fromClient].SendIntoGame(username, team);
    }

    public static void PlayerMovement(Guid fromClient, Packet packet)
    {
        bool[] inputs = new bool[packet.ReadInt()];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = packet.ReadBool();
        }

        Quaternion rotation = packet.ReadQuaternion();

        Room.Clients[fromClient].player.SetInput(inputs, rotation);
    }

    public static void PlayerShooting(Guid fromClient, Packet packet)
    {
        var duraction = packet.ReadVector3();

        Room.Clients[fromClient].player.Shoot(duraction);
    }

    public static void PlayerThrowItem(Guid fromClient, Packet packet)
    {
        var direction = packet.ReadVector3();

        Room.Clients[fromClient].player.ThrowItem(direction);
    }

    public static void PlayerChangeWeapon(Guid fromClient, Packet packet)
    {
        var leftOrRigth = packet.ReadInt();

        Room.Clients[fromClient].player.ChooseWeapon(leftOrRigth);
    }

    public static void PlayerRespawn(Guid fromClient, Packet packet)
    {
        Room.GetClient(fromClient).player.Suicide();
    }
}
