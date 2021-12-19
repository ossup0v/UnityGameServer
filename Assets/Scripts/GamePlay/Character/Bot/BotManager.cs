using System.Collections.Generic;

public class BotManager
{
    public static int MaxBotCount = 50;
    private static Dictionary<int, BotBase> _bots = new Dictionary<int, BotBase>();
    private static int nextBotId = 1;

    public static int GetNextId()
    {
        nextBotId++;
        return nextBotId;
    }

    public static IReadOnlyDictionary<int, BotBase> GetBots() 
    {
        return _bots;
    }

    public static void AddBot(int id, BotBase bot)
    {
        _bots.Add(id, bot);
    }

    public static void RemoveBot(int id)
    {
        _bots.Remove(id);
    }

    public static int GetBotCount()
    {
        return _bots.Count;
    }
}
