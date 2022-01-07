using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance;
    public Dictionary<int, List<Vector3>> SpawnPoints = new Dictionary<int, List<Vector3>>();

    private void Awake()
    {
        Instance = this;

        var spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();

        foreach (var spawnPoint in spawnPoints)
        {
            if (!SpawnPoints.ContainsKey(spawnPoint.Team))
                SpawnPoints.Add(spawnPoint.Team, new List<Vector3>());

            SpawnPoints[spawnPoint.Team].Add(spawnPoint.transform.position);
        }
    }

    public Vector3 GetRandomSpawnPoint(int team)
    { 
        return SpawnPoints[team][Random.Range(0, SpawnPoints[team].Count)];
    }
}
