using UnityEngine;

public class HitRegistration : MonoBehaviour
{
    public CharacterBase defender;

    public void RegisterHit(float damage, CharacterBase attacker, float impactForce = 0, Vector3 normal = default(Vector3))
    {
        // bot can't attack bot
        if ((defender.CharacterKind == CharacterKind.bot && attacker.CharacterKind != CharacterKind.bot) 
            //every only can attack player (
            || defender.CharacterKind == CharacterKind.player)
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
