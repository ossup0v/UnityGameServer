using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; } = new NetworkManager();
    public int Port = 26950;

    public GameObject PlayerPrefab;
    public GameObject ProjectilePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"duplicated instances of {nameof(NetworkManager)}");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Server.Start(50, Port);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(PlayerPrefab, new Vector3(0, 1f, 0), Quaternion.identity).GetComponent<Player>();
    }

    public Projectile InstantiatePrjectile(Transform shootOrigin)
    {
        return Instantiate(ProjectilePrefab, shootOrigin.position + shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }
}
