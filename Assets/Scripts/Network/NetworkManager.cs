using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }
    public int Port = 26950;

    public GameObject BotPrefab;
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
        var port = Port;
        try
        {
            var args = Environment.GetCommandLineArgs();
            port = int.Parse(args[1]);
        }
        catch (Exception)
        {

        }
        Server.Start(50, port);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public BotBase InstantiateBot(Vector3 spawnPoint)
    {
        return Instantiate(BotPrefab, spawnPoint, Quaternion.identity).GetComponent<BotBase>();
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
