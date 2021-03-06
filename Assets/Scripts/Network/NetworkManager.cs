using System;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }
    private int ClientsPort = 26954;
    public int ServerPort = 26949;

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
        var clientsPort = ClientsPort;
        var serverPort = ServerPort;
        var metagameRoomId = default(Guid);
        var maxPlayersCount = 1;
        try
        {
#warning ??, ?? ??? ?????? ????, ?????? ????????? newtonsoft.json, ??????????
            var args = Environment.GetCommandLineArgs();
            Debug.Log(string.Join(" ", args));
            var argsArray = args[1].Split(';');
            clientsPort = int.Parse(argsArray[0]);
            serverPort = int.Parse(argsArray[1]);
            metagameRoomId = Guid.Parse(argsArray[2]);
            maxPlayersCount = int.Parse(argsArray[3]);
        }
        catch (Exception)
        {
            Debug.LogError("can't read environment args");
        }

        Debug.Log($"Game created by server, metagame room Id {metagameRoomId}");
        Room.Start(maxPlayersCount, clientsPort, serverPort, metagameRoomId);
    }

    private void OnApplicationQuit()
    {
        Room.Stop();
    }

    public DefaultBot InstantiateBot(Vector3 spawnPoint)
    {
        return Instantiate(BotPrefab, spawnPoint, Quaternion.identity).GetComponent<DefaultBot>();
    }

    public Player InstantiatePlayer(int team)
    {
        return Instantiate(PlayerPrefab, SpawnPointManager.Instance.GetRandomSpawnPoint(team), Quaternion.identity).GetComponent<Player>();
    }

    public Projectile InstantiatePrjectile(Transform shootOrigin)
    {
        return Instantiate(ProjectilePrefab, shootOrigin.position + shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }
}
