using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayersManager
{
    public static Dictionary<Guid, PlayerMetagameData> Players = new Dictionary<Guid, PlayerMetagameData>();

    public static void AddPlayerData(PlayerMetagameData data)
    {
        if (Players.ContainsKey(data.Id))
        {
            Debug.LogError($"duplicated client with data {data}");
            return;
        }

        Players.Add(data.Id, data);
    }

    public static PlayerMetagameData GetPlayer(Guid id)
    {
        if (!Players.ContainsKey(id))
        {
            Debug.LogError($"can't find player with id {id}, all players is {Players.ToDebugLog()}");
            return null;
        }

        return Players[id];
    }
}
