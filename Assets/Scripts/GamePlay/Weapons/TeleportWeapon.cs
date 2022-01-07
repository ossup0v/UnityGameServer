using UnityEngine;

public class TeleportWeapon : WeaponBase
{
    public TeleportWeapon()
    {
        Radius = 1000;
        _timer = new Timer(0.15f);
    }

    public override WeaponKind Kind => WeaponKind.Teleport;

    public override void Shoot(CharacterBase owner, Vector3 duraction, Vector3 from)
    {
        if (_timer.ResetIfElapsed())
        {
            SendShoot(owner);
            if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
            {
                owner.MoveTo(hit.point);
            }
        }
    }
}
