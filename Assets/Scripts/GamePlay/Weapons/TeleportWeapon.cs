using UnityEngine;

public class TeleportWeapon : WeaponBase
{
    public TeleportWeapon()
    {
        Radius = 100;
    }

    public override WeaponKind Kind => WeaponKind.Teleport;

    public override void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from)
    {
        SendShoot(owner);
        if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
        {
            owner.MoveTo(hit.point);
        }
    }
}
