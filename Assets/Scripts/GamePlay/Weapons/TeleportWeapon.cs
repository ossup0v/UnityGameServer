using UnityEngine;

public class TeleportWeapon : WeaponBase
{
    public TeleportWeapon(CharacterBase owner) : base(owner)
    {
        Radius = 1000;
        MaxBulletAmount = 1;
        CurrentBulletAmount = 1;
        BulletAmountToShoot = 0;
        _timer = new Timer(0.15f);
    }

    public override WeaponKind Kind => WeaponKind.Teleport;

    public override void Shoot(Vector3 duraction, Vector3 from)
    {
        if (IsCanShootIfCanSubsctractBullets())
        {
            SendShoot();
            if (Physics.Raycast(from, duraction, out var hit, GetRadius()))
            {
                Owner.MoveTo(hit.point);
            }
        }
    }
}
