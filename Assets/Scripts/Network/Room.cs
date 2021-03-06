using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Room
{
    public static Dictionary<Guid, Client> Clients = new Dictionary<Guid, Client>();
    public delegate void PacketHandler(Guid fromClient, Packet packet);
    public static Dictionary<int, PacketHandler> packetHandlers;
    private static NetworkListener _clientsListener;
    private static Dictionary<int, Vector3> _spawnPoints = new Dictionary<int, Vector3>();

    public static NetworkClient Server;
    public static Guid RoomId;
    public static Guid MetagameRoomId;

    public static int PlayersAmount;
    public static int PortForClients;
    public static int PortForServer;

    public static Client GetClient(Guid clientId)
    {
        Clients.TryGetValue(clientId, out var client);
        return client;
    }

    public static void Start(int maxPlayers, 
        int portForClients, 
        int portForServer, 
        Guid metagameRoomId)
    {
        PlayersAmount = maxPlayers;
        PortForClients = portForClients;
        PortForServer = portForServer;
        MetagameRoomId = metagameRoomId;

        Debug.Log("Starting server...");
        InitializeServerData();

        Server = new NetworkClient();
        Server.ConnectToServer("127.0.0.1", PortForServer);

        _clientsListener = new NetworkListener(PortForClients, TCPClientConnectCallback, UDPReceiveClientsCallback);

        _clientsListener.StartListen();

        Debug.Log($"Server started on port {PortForClients}.");
    }

    public static void Stop()
    {
        _clientsListener.StopListen();
    }

    private static void TCPClientConnectCallback(IAsyncResult result)
    {
        var client = default(TcpClient);
        try
        {
            client = _clientsListener.GetTcpListener().EndAcceptTcpClient(result);
            _clientsListener.GetTcpListener().BeginAcceptTcpClient(TCPClientConnectCallback, null);
        }
        catch (Exception ex) { }

        Debug.Log($"Incoming connection from {client.Client.RemoteEndPoint}...");

        if (Clients.Count < PlayersAmount)
        {
            var newGuid = Guid.NewGuid();
            var newClient = new Client(newGuid);
            Clients.Add(newGuid, newClient);
            newClient.tcp.Connect(client);
            return;
        }

        Debug.LogError($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private static void UDPReceiveClientsCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = _clientsListener.GetUdpListener().EndReceive(_result, ref clientEndPoint);
            _clientsListener.GetUdpListener().BeginReceive(UDPReceiveClientsCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using (Packet packet = new Packet(data))
            {
                Guid clientId = packet.ReadGuid();

                if (clientId == default(Guid))
                {
                    return;
                }

                //process client udp packets here
                if (Clients[clientId].udp.EndPoint == null)
                {
                    Clients[clientId].udp.Connect(clientEndPoint);
                }

                if (Clients[clientId].udp.EndPoint.ToString() == clientEndPoint.ToString())
                {
                    Clients[clientId].udp.HandleData(packet);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving UDP data: {ex}");
        }
    }

    public static void SendClientUDPData(IPEndPoint clientEndPoint, Packet packet)
    {
        try
        {
            if (clientEndPoint != null)
            {
                _clientsListener.GetUdpListener().BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to {clientEndPoint} via UDP: {ex}");
        }
    }

    private static void InitializeServerData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ToGameRoom.welcomeReceived, RoomClientHandler.WelcomeReceived },
                { (int)ToGameRoom.playerMovement, RoomClientHandler.PlayerMovement },
                { (int)ToGameRoom.playerShooting, RoomClientHandler.PlayerShooting },
                { (int)ToGameRoom.playerThrowItem, RoomClientHandler.PlayerThrowItem },
                { (int)ToGameRoom.playerChangeWeapon, RoomClientHandler.PlayerChangeWeapon },
                { (int)ToGameRoom.playerRespawn, RoomClientHandler.PlayerRespawn },
            };

        Debug.Log("Initialized packets.");
    }
}
