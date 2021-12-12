﻿using UnityEngine;

public class GunWeapon : WeaponBase
{
    public GunWeapon()
    {
        Damage = 20;
        Radius = 25;
        RadiusOfDamage = 10;
    }

    public override WeaponKind Kind { get; } = WeaponKind.Gun;

    public override void Shoot(Player owner, Vector3 duraction, Vector3 from)
    {
        ServerSend.PlayerShootUDP(owner);
        if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
        {
            if (hit.collider.TryGetComponent<HitRegistration>(out var hitRegistration))
            {
                hitRegistration.RegisterHit(GetDamage(owner), owner.Id);
                ServerSend.PlayerHitTCP(owner, Kind, hit.transform.position);
                return;
            }

            ServerSend.PlayerHitUDP(owner, Kind, hit.transform.position);
        }
    }
}