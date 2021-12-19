using UnityEngine;

public class RocketLaucnherWeapon : WeaponBase
{
    public RocketLaucnherWeapon()
    {
        Damage = 50;
        Radius = 100;
        RadiusOfDamage = 2f;
        ImpactForce = 100f;
    }

    public override WeaponKind Kind => WeaponKind.RocketLauncher;

    public override void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from)
    {
        SendShoot(owner);
        if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.point, RadiusOfDamage);
            var hited = false;
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<HitRegistration>(out var hitRegistration))
                {
                    hitRegistration.RegisterHit(GetDamage(owner), owner, ImpactForce, hit.normal);
                    hited = true;
                }
            }

            if (hited)
            {
                SendHit(owner, Kind, hit.point);
            }
            else
            {
                SendHit(owner, Kind, hit.point);
            }
        }
    }
}
