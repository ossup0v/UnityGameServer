using UnityEngine;

public abstract class WeaponBase
{
    public abstract WeaponKind Kind { get; }
    public bool IsBulletsFull => MaxBulletAmount <= CurrentBulletAmount;

    public float Damage;
    public float Radius;
    public float RadiusOfDamage;
    public float ImpactForce;

    public int CurrentBulletAmount;
    public int MaxBulletAmount;
    public int BulletAmountToShoot;
    public CharacterBase Owner { get; private set; }

    public WeaponBase(CharacterBase owner)
    {
        Owner = owner;
    }

    protected Timer _timer;

    protected float GetDamage()
    {
        return Damage * Owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponDamageBooster);
    }

    protected float GetRadius()
    {
        return Radius * Owner.BoosterContainer.GetBoosterValue(BoosterKind.weaponRadiusBooster);
    }

    public abstract void Shoot(Vector3 duraction, Vector3 from);

    protected bool IsCanShootIfCanSubsctractBullets(bool isCheater = false)
    {
        var isCanShoot = isCheater ||
            (_timer.ResetIfElapsed() && CurrentBulletAmount >= BulletAmountToShoot);

        if (isCanShoot && !isCheater)
        {
            CurrentBulletAmount -= BulletAmountToShoot;

            if (Owner.Human)
                RoomSendClient.PlayerBulletAmount(Owner.Id, this);
        }

        return isCanShoot;
    }

    public void AddBullets(int amount)
    {
        CurrentBulletAmount += amount;

        if (CurrentBulletAmount > MaxBulletAmount)
            CurrentBulletAmount = MaxBulletAmount;
    }

    //удали этот говно код, плс
    protected void SendShoot()
    {
        if (Owner.CharacterKind == CharacterKind.player)
            RoomSendClient.PlayerShootTCP(Owner);
        if (Owner.CharacterKind == CharacterKind.bot)
            RoomSendClient.BotShoot(Owner);
    }

#warning боже, как же это плохо..
    protected void SendHit(Vector3 pos, bool isHited)
    {
        if (isHited)
        {
            if (Owner.CharacterKind == CharacterKind.player)
                RoomSendClient.PlayerHitTCP(Owner, Kind, pos);
            if (Owner.CharacterKind == CharacterKind.bot)
                RoomSendClient.BotHit(Owner, Kind, pos);
        }
        else 
        {
            if (Owner.CharacterKind == CharacterKind.player)
                RoomSendClient.PlayerHitUDP(Owner, Kind, pos);
            if (Owner.CharacterKind == CharacterKind.bot)
                RoomSendClient.BotHit(Owner, Kind, pos);
        }
    }
}
