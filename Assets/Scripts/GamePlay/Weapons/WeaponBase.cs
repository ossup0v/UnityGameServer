using UnityEngine;

public abstract class WeaponBase
{
    public abstract WeaponKind Kind { get; }
    public float Damage;
    public float Radius;
    public float RadiusOfDamage;
    public float ImpactForce;
    protected Timer _timer;

    protected float GetDamage(CharacterBase owner)
    {
        return Damage * owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponDamageBooster);
    }

    protected float GetRadius(CharacterBase owner)
    {
        return Radius * owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponRadiusBooster);
    }

    public abstract void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from);

    //удали этот говно код, плс
    protected void SendShoot(CharacterBase owner)
    {
        if (owner.CharacterKind == CharacterKind.player)
            RoomSendClient.PlayerShootTCP(owner);
        if (owner.CharacterKind == CharacterKind.bot)
            RoomSendClient.BotShoot(owner);
    }

#warning боже, как же это плохо..
    protected void SendHit(CharacterBase owner, WeaponKind weaponKind, Vector3 pos, bool isHited)
    {
        if (isHited)
        {
            if (owner.CharacterKind == CharacterKind.player)
                RoomSendClient.PlayerHitTCP(owner, weaponKind, pos);
            if (owner.CharacterKind == CharacterKind.bot)
                RoomSendClient.BotHit(owner, weaponKind, pos);
        }
        else 
        {
            if (owner.CharacterKind == CharacterKind.player)
                RoomSendClient.PlayerHitUDP(owner, weaponKind, pos);
            if (owner.CharacterKind == CharacterKind.bot)
                RoomSendClient.BotHit(owner, weaponKind, pos);
        }
    }
}
