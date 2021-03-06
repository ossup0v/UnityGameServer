using System;
using System.Linq;
using UnityEngine;

public class RoomClientHandler
{
    public static void WelcomeReceived(Guid fromClient, Packet packet)
    {
        Debug.Log("Welcome received");
        
        Guid clientIdCheck = packet.ReadGuid();
        
        Room.Clients[fromClient].SetNewId(clientIdCheck);

        Room.Clients.MoveValueToOtherKey(fromClient, clientIdCheck);

        PlayersManager.Players.MoveValueToOtherKey(fromClient, clientIdCheck);

        string username = PlayersManager.GetPlayer(clientIdCheck)?.Username ?? "TEST1";
        int team = PlayersManager.GetPlayer(clientIdCheck)?.Team ?? 1;

        Debug.Log($"User {fromClient} team is {team}");
        Debug.Log($"{Room.Clients[clientIdCheck].tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }

        Room.Clients[clientIdCheck].SendIntoGame(username, team);

        if (RatingManager.Rating.Values.Count == Room.PlayersAmount)
        {
            Debug.Log($"rating table");
            Debug.Log(string.Join(" ", RatingManager.Rating.Values.Select(x => $"username {x.Username}; team {x.Team}")));
            RoomSendClient.UpdateFullRatingTable(RatingManager.Rating);

            UnityGame.Instance.StartGame();
        }
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
