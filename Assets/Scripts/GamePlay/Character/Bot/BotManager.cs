using System;
using System.Collections.Generic;

public class BotManager
{
    public static int MaxBotCount = 6;
    private static Dictionary<Guid, BotBase> _bots = new Dictionary<Guid, BotBase>();

    public static Guid GetNextId()
    {
        return Guid.NewGuid();
    }

    public static IReadOnlyDictionary<Guid, BotBase> GetBots() 
    {
        return _bots;
    }

    public static void AddBot(Guid id, BotBase bot)
    {
        _bots.Add(id, bot);
    }

    public static void RemoveBot(Guid id)
    {
        _bots.Remove(id);
    }

    public static int GetBotCount()
    {
        return _bots.Count;
    }
}
