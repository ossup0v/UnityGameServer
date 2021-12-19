using System.Collections.Generic;

public class BotManager
{
    public static int MaxBots = 10;
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
}
