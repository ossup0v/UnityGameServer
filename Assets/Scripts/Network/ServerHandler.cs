using System;
using UnityEngine;

public class ServerHandler
{
    public static void WelcomeReceived(Guid fromClient, Packet packet)
    {
        Debug.Log("Welcome received");
        Guid clientIdCheck = packet.ReadGuid();
        string username = packet.ReadString();

        Debug.Log($"Welcome received from id on server {fromClient}, in packet {clientIdCheck}");
        //Server.clients.Remove(fromClient);
        //var newClient = new Client(clientIdCheck);
        //
        //Server.clients.Add(clientIdCheck, );
        Debug.Log($"{Server.clients[fromClient].tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }
        Server.clients[fromClient].SendIntoGame(username);
    }

    public static void PlayerMovement(Guid fromClient, Packet packet)
    {
        bool[] inputs = new bool[packet.ReadInt()];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = packet.ReadBool();
        }

        Quaternion rotation = packet.ReadQuaternion();

        Server.clients[fromClient].player.SetInput(inputs, rotation);
    }

    public static void PlayerShooting(Guid fromClient, Packet packet)
    {
        var duraction = packet.ReadVector3();

        Server.clients[fromClient].player.Shoot(duraction);
    }

    public static void PlayerThrowItem(Guid fromClient, Packet packet)
    {
        var direction = packet.ReadVector3();

        Server.clients[fromClient].player.ThrowItem(direction);
    }

    public static void PlayerChangeWeapon(Guid fromClient, Packet packet)
    {
        var leftOrRigth = packet.ReadInt();

        Server.clients[fromClient].player.ChooseWeapon(leftOrRigth);
    }

    public static void PlayerRespawn(Guid fromClient, Packet packet)
    {
        Server.GetClient(fromClient).player.Suicide();
    }
}
