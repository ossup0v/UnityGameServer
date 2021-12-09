using UnityEngine;

public class HitRegistration : MonoBehaviour
{
    public Player player;

    public void RegisterHit(float damage)
    { 
        player.TakeDamage(damage);   
    }
}
