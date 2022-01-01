using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> ItemSpawners = new Dictionary<int, ItemSpawner>();

    private static int nextSpawnerId = 1;

    public int SpawnerId;
    public bool HasItem = false;
    private Collider selfCollider;
    private void Start()
    {
        HasItem = false;
        SpawnerId = nextSpawnerId;
        nextSpawnerId++;
        ItemSpawners.Add(SpawnerId, this);
        selfCollider = GetComponent<Collider>();

        StartCoroutine(SpawnItem());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasItem && other.TryGetComponent<Player>(out var player))
            if (player.AttemptPickupItem())
            {
                selfCollider.enabled = false;
                ItemPickup(player.Id);
            }
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(5f);

        selfCollider.enabled = true;
        HasItem = true;
        RoomSendClient.ItemSpawned(SpawnerId);
    }

    private void ItemPickup(Guid playerId)
    {
        HasItem = false;
        RoomSendClient.ItemPickup(SpawnerId, playerId);
        StartCoroutine(SpawnItem());
    }
}
