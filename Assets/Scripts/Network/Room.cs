using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Room
{
    public static int MaxPlayers { get; private set; }
    public static int PortForClients { get; private set; }
    public static int PortForServer { get; private set; }
    public static Dictionary<Guid, Client> clients = new Dictionary<Guid, Client>();
    public delegate void PacketHandler(Guid fromClient, Packet packet);
    public static Dictionary<int, PacketHandler> packetHandlers;
    private static NetworkListener _clientsListener;

    public static string Mode;
    public static string Title;

    public static NetworkClient Server;
    public static Guid RoomId;
    public static Guid CreatorId;

    public static Client GetClient(Guid clientId)
    {
        clients.TryGetValue(clientId, out var client);
        return client;
    }

    public static void Start(int maxPlayers, 
        int portForClients, 
        int portForServer, 
        Guid roomId,
        Guid creatorId,
        string mode,
        string title)
    {
        MaxPlayers = maxPlayers;
        PortForClients = portForClients;
        PortForServer = portForServer;
        RoomId = roomId;
        CreatorId = creatorId;
        Title = title;
        Mode = mode;

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

        if (clients.Count < MaxPlayers)
        {
            var newGuid = Guid.NewGuid();
            var newClient = new Client(newGuid);
            clients.Add(newGuid, newClient);
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
                if (clients[clientId].udp.EndPoint == null)
                {
                    clients[clientId].udp.Connect(clientEndPoint);
                }

                if (clients[clientId].udp.EndPoint.ToString() == clientEndPoint.ToString())
                {
                    clients[clientId].udp.HandleData(packet);
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
