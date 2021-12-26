using System;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public GunWeapon()
    {
        Damage = 20;
        Radius = 100;
        RadiusOfDamage = 10;
        _timer = new Timer(TimeSpan.FromSeconds(0.20f));
    }

    public override WeaponKind Kind { get; } = WeaponKind.Gun;

    public override void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from)
    {
        if (_timer.ResetIfElapsed())
        {
            SendShoot(owner);
            if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
            {
                var isHited = false;
                if (hit.collider.TryGetComponent<HitRegistration>(out var hitRegistration))
                {
                    hitRegistration.RegisterHit(GetDamage(owner), owner);
                    isHited = true;
                }

                SendHit(owner, Kind, hit.point, isHited);
            }
        }
    }
}