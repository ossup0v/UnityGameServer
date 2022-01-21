using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions
{
    public static string ToDebugLog<TKey, TValue>(this Dictionary<TKey, TValue> kvp)
        => string.Join(";", kvp.Values.Select(x => x.ToString()));

    public static void MoveValueToOtherKey<TKey, TValue>(this Dictionary<TKey, TValue> kvp, TKey old, TKey @new)
    {
        if (!kvp.ContainsKey(old))
            return;

        var @value = kvp[old];

        kvp.Remove(old);

        kvp.Add(@new, @value);
    }
}
