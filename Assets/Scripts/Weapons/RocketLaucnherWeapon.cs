using UnityEngine;

public class RocketLaucnherWeapon : WeaponBase
{
    public RocketLaucnherWeapon()
    {
        Damage = 50;
        Radius = 100;
        RadiusOfDamage = 10;
        ImpactForce = 60f;
    }

    public override WeaponKind Kind => WeaponKind.RocketLauncher;

    public override void Shoot(Player owner, Vector3 duraction, Vector3 from)
    {
        ServerSend.PlayerShootUDP(owner);
        if (Physics.Raycast(from, duraction, out var hit, GetRadius(owner)))
        {
            if (hit.collider.TryGetComponent<HitRegistration>(out var hitRegistration))
            {
                hitRegistration.RegisterHit(GetDamage(owner), owner.Id);
                ServerSend.PlayerHitTCP(owner, Kind, hit.point);
                return;

                //TODO do this from update player position
                //if (hit.rigidbody != null)
                //{
                //    hit.rigidbody.AddForce(-hit.normal * ImpactForce);
                //}
            }

            ServerSend.PlayerHitUDP(owner, Kind, hit.point);
        }
    }
}
