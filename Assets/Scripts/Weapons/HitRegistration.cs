using UnityEngine;

public class HitRegistration : MonoBehaviour
{
    public Player player;

    public void RegisterHit(float damage, int attackerPlayerId)
    { 
        player.TakeDamage(damage, attackerPlayerId);   
    }
}
