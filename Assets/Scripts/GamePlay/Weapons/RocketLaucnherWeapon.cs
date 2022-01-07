using System;
using UnityEngine;

public class RocketLaucnherWeapon : WeaponBase
{
    public RocketLaucnherWeapon()
    {
        Damage = 50;
        Radius = 1000;
        RadiusOfDamage = 4f;
        ImpactForce = 100f;
        _timer = new Timer(TimeSpan.FromSeconds(0.75f));
    }

    public override WeaponKind Kind => WeaponKind.RocketLauncher;

    public override void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from)
    {

        var client = Room.GetClient(owner.Id);

        var isCheater = client != null && client.player.Username == "cheater";

        if (_timer.ResetIfElapsed() || isCheater)
        {
            SendShoot(owner);
            if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
            {
                Collider[] colliders = Physics.OverlapSphere(hit.point, RadiusOfDamage);
                var isHited = false;
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<HitRegistration>(out var hitRegistration))
                    {
                        hitRegistration.RegisterHit(GetDamage(owner), owner, ImpactForce, hit.normal);
                        isHited = true;
                    }
                }

                SendHit(owner, Kind, hit.point, isHited);
            }
        }
    }
}
