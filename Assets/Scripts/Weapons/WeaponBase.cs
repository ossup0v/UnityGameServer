using UnityEngine;

public abstract class WeaponBase
{
    public abstract WeaponKind Kind { get; }
    public float Damage;
    public float Radius;
    public float RadiusOfDamage;
    public float ImpactForce;

    protected float GetDamage(Player owner)
    {
        return Damage * owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponDamageBooster);
    }

    protected float GetRadius(Player owner)
    { 
        return Radius * owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponRadiusBooster);
    }

    public abstract void Shoot(Player owner, Vector3 duraction, Vector3 from);
}
