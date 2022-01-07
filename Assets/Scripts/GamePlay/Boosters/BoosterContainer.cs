using System.Collections.Generic;

public class BoosterContainer
{
    private Dictionary<BoosterKind, float> boosters = new Dictionary<BoosterKind, float>()
    {
        [BoosterKind.weaponDamageBooster] = 1f,
        [BoosterKind.weaponRadiusBooster] = 1f,
    };

    public float GetBoosterValue(BoosterKind kind)
    {
        boosters.TryGetValue(kind, out var value);
        return value;
    }

    public void SetBooster(BoosterKind kind, float value)
    {
        if (!boosters.ContainsKey(kind))
            boosters.Add(kind, value);
        else
            boosters[kind] = value;
    }

    public void IncreaseBooster(BoosterKind kind, float value)
    {
        if (!boosters.ContainsKey(kind))
            boosters.Add(kind, value);
        else
            boosters[kind] += value;
    }
}