using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> ItemSpawners = new Dictionary<int, ItemSpawner>();

    private static int nextSpawnerId = 1;

    public int SpawnerId;
    public bool HasItem = false;

    private void Start()
    {
        HasItem = false;
        SpawnerId = nextSpawnerId;
        nextSpawnerId++;
        ItemSpawners.Add(SpawnerId, this);

        StartCoroutine(SpawnItem());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasItem && other.TryGetComponent<Player>(out var player))
        {
            if (player.AttemptPickupItem())
            {
                ItemPickup(player.Id);
            }
        }
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(5f);

        HasItem = true;
        ServerSend.ItemSpawned(SpawnerId);
    }

    private void ItemPickup(int playerId)
    {
        HasItem = false;
        ServerSend.ItemPickup(SpawnerId, playerId);
        StartCoroutine(SpawnItem());
    }
}
