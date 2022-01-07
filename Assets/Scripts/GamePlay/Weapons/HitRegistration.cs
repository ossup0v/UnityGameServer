using UnityEngine;

public class HitRegistration : MonoBehaviour
{
    public CharacterBase defender;

    public void RegisterHit(float damage, CharacterBase attacker, float impactForce = 0, Vector3 normal = default(Vector3))
    {
        if (attacker.IsCanAttackOther(defender))
        {
            defender.TakeDamage(damage, attacker);
        }

        if (impactForce != 0 && normal != default(Vector3))
        {
            //make here 
            //if (player.TryGetComponent<Rigidbody>(out var rb))
            //{
            //    rb.AddForce(-normal * impactForce);
            //    ServerSend.PlayerPosition(player);
            //}
        }
    }
}
