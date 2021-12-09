using UnityEngine;

public class ServerHandler
{
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        int clientIdCheck = packet.ReadInt();
        string username = packet.ReadString();

        Debug.Log($"{Server.clients[fromClient].tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }
        Server.clients[fromClient].SendIntoGame(username);
    }

    public static void PlayerMovement(int fromClient, Packet packet)
    {
        bool[] inputs = new bool[packet.ReadInt()];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = packet.ReadBool();
        }

        Quaternion rotation = packet.ReadQuaternion();

        Server.clients[fromClient].player.SetInput(inputs, rotation);
    }

    public static void PlayerShooting(int fromClient, Packet packet)
    {
        var duraction = packet.ReadVector3();

        Server.clients[fromClient].player.Shoot(duraction);
    }

    public static void PlayerThrowItem(int fromClient, Packet packet)
    {
        var direction = packet.ReadVector3();

        Server.clients[fromClient].player.ThrowItem(direction);
    }
}
