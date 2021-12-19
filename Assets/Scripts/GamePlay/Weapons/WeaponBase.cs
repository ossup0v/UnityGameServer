using UnityEngine;

public abstract class WeaponBase
{
    public abstract WeaponKind Kind { get; }
    public float Damage;
    public float Radius;
    public float RadiusOfDamage;
    public float ImpactForce;

    protected float GetDamage(CharacterBase owner)
    {
        return Damage * owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponDamageBooster);
    }

    protected float GetRadius(CharacterBase owner)
    { 
        return Radius * owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponRadiusBooster);
    }

    public abstract void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from);
}
