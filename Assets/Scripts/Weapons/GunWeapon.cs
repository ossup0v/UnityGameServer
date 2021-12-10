using UnityEngine;

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
        if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
        {
            if (hit.collider.TryGetComponent<HitRegistration>(out var hitRegistration))
            {
                hitRegistration.RegisterHit(GetDamage(owner));
            }
        }
    }
}