using System.Collections.Generic;

public class RatingManager
{
    public static Dictionary<int, RatingEntity> Rating = new Dictionary<int, RatingEntity>();

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
        });
    }

    public static void KillAndDeath(int playerKillerId, int playerDieId)
    {
        AddKill(playerKillerId);
        AddDeath(playerDieId);
    }

    private static void AddKill(int playerId)
    {
        if (!Rating.ContainsKey(playerId))
        {
            System.Console.WriteLine("ERROR add kill!");
            return;
        }

        Rating[playerId].Killed++;
    }

    public static void AddDeath(int playerId)
    {
        if (!Rating.ContainsKey(playerId))
        {
            System.Console.WriteLine("ERROR add death!");
            return;
        }

        Rating[playerId].Died++;
    }
}
