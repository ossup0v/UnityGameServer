using UnityEngine;

public class GunWeapon : WeaponBase
{
    public GunWeapon()
    {
        Damage = 20;
        Radius = 100;
        RadiusOfDamage = 10;
    }

    public override WeaponKind Kind { get; } = WeaponKind.Gun;

    public override void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from)
    {
        SendShoot(owner);
        if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
        {
            if (hit.collider.TryGetComponent<HitRegistration>(out var hitRegistration))
            {
                hitRegistration.RegisterHit(GetDamage(owner), owner);
                SendHit(owner, Kind, hit.point);
                return;
            }

            SendHit(owner, Kind, hit.point);
        }
    }
}