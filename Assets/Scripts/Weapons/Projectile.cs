using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Dictionary<int, Projectile> Projectiles = new Dictionary<int,Projectile>();
    private static int nextPjectileId = 1;

    public int Id;
    public Rigidbody Body;
    public int ThrowedByPlayer;
    public Vector3 initialForce;
    public float explotionRaduios = 1.5f;
    public float explotionDamage = 75f;

    private void Start()
    {
        Id = nextPjectileId;
        nextPjectileId++;
        Projectiles.Add(Id, this);
        Body.AddForce(initialForce);
        ServerSend.SpawnProjectile(this, ThrowedByPlayer);
        StartCoroutine(ExplodeAfterTime());
    }

    private void FixedUpdate()
    {
        ServerSend.ProjectilePosition(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    public void Initialize(Vector3 initialMovementDuraction, float initialForceStrength, int throwedByPlayer)
    {
        initialForce = initialForceStrength * initialMovementDuraction;
        ThrowedByPlayer = throwedByPlayer; 
    }

    private void Explode()
    {
        ServerSend.ProjectileExploded(this);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explotionRaduios);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Player>(out var player))
            {
                player.TakeDamage(explotionDamage, ThrowedByPlayer);
            }
        }

        Projectiles.Remove(Id);
        Destroy(gameObject);
    }
    
    private IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(4f);
        Explode();
    }
}
