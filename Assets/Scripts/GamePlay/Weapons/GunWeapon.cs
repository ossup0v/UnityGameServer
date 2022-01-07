using System;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public GunWeapon(CharacterBase owner) : base(owner)
    {
        Damage = 20;
        Radius = 100;
        RadiusOfDamage = 10;
        MaxBulletAmount = 30;
        CurrentBulletAmount = 30;
        BulletAmountToShoot = 1;
        _timer = new Timer(TimeSpan.FromSeconds(0.20f));
    }

    public override WeaponKind Kind { get; } = WeaponKind.Gun;

    public override void Shoot(Vector3 duraction, Vector3 from)
    {
        if (IsCanShootIfCanSubsctractBullets())
        {
            SendShoot();
            if (Physics.Raycast(from, duraction, out var hit, GetRadius()))
            {
                var isHited = false;
                if (hit.collider.TryGetComponent<HitRegistration>(out var hitRegistration))
                {
                    hitRegistration.RegisterHit(GetDamage(), Owner);
                    isHited = true;
                }

                SendHit(hit.point, isHited);
            }
        }
    }
}