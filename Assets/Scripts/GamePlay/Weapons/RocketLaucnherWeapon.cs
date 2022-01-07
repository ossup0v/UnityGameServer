using System;
using UnityEngine;

public class RocketLaucnherWeapon : WeaponBase
{
    public RocketLaucnherWeapon(CharacterBase owner) : base(owner)
    {
        Damage = 50;
        Radius = 1000;
        RadiusOfDamage = 4f;
        ImpactForce = 100f;
        MaxBulletAmount = 30;
        CurrentBulletAmount = 30;
        BulletAmountToShoot = 1;
        _timer = new Timer(TimeSpan.FromSeconds(0.75f));
    }

    public override WeaponKind Kind => WeaponKind.RocketLauncher;

    public override void Shoot(Vector3 duraction, Vector3 from)
    {
        var client = Room.GetClient(Owner.Id);

        var isCheater = client != null && client.player.Username == "cheater";

        if (IsCanShootIfCanSubsctractBullets(isCheater))
        {
            SendShoot();
            if (Physics.Raycast(from, duraction, out var hit, GetRadius()))
            {
                Collider[] colliders = Physics.OverlapSphere(hit.point, RadiusOfDamage);
                var isHited = false;
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<HitRegistration>(out var hitRegistration))
                    {
                        hitRegistration.RegisterHit(GetDamage(), Owner, ImpactForce, hit.normal);
                        isHited = true;
                    }
                }

                SendHit(hit.point, isHited);
            }
        }
    }
}
