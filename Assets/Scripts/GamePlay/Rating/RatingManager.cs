using System;
using System.Collections.Generic;
using UnityEngine;

public class RatingManager
{
    public static Dictionary<Guid, RatingEntity> Rating = new Dictionary<Guid, RatingEntity>();

    public static void RemovePlayer(Player player)
    {
        Rating.Remove(player.Id);
    }

    public static void InitPlayer(Player player)
    {
        Rating.Add(player.Id, new RatingEntity
        {
            Username = player.Username,
            PlayerId = player.Id,
            Killed = 0,
            Died = 0,
            Team = player.Team,
        });
    }

    public static void KillAndDeath(Guid playerKillerId, Guid playerDieId)
    {
        AddKill(playerKillerId);
        AddDeath(playerDieId);
    }

    private static void AddKill(Guid playerId)
    {
        if (!Rating.ContainsKey(playerId))
        {
            Debug.LogError("ERROR add kill!");
            return;
        }

        Rating[playerId].Killed++;
    }

    public static void AddKillBot(Guid playerId)
    {
        if (!Rating.ContainsKey(playerId))
        {
            Debug.LogError("ERROR add AddBot!");
            return;
        }

        Rating[playerId].KilledBots++;
    }

    public static void AddDeath(Guid playerId)
    {
        if (!Rating.ContainsKey(playerId))
        {
            Debug.LogError("ERROR add death!");
            return;
        }

        Rating[playerId].Died++;
    }

    public static RatingEntity GetPlayerEntity(Guid playerId)
    {
        return Rating[playerId];
    }
}
