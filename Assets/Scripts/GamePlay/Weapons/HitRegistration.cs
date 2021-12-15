using UnityEngine;

public class HitRegistration : MonoBehaviour
{
    public Player player;

    public void RegisterHit(float damage, int attackerPlayerId, float impactForce = 0, Vector3 normal = default(Vector3))
    {
        player.TakeDamage(damage, attackerPlayerId);

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
