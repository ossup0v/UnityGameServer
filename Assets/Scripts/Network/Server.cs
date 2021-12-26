using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server 
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<Guid, Client> clients = new Dictionary<Guid, Client>();
    public delegate void PacketHandler(Guid fromClient, Packet packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static Client GetClient(Guid clientId)
    {
        clients.TryGetValue(clientId, out var client);
        return client;
    }

    public static void Start(int maxPlayers, int port)
    {
        MaxPlayers = maxPlayers;
        Port = port;

        Debug.Log("Starting server...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server started on port {Port}.");
    }

    internal static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }

    private static void TCPConnectCallback(IAsyncResult result)
    {
        var client = default(TcpClient);
        try
        {
            client = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
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

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpListener.EndReceive(_result, ref clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

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

                if (clients[clientId].udp.EndPoint == null)
                {
                    clients[clientId].udp.Connect(clientEndPoint);
                    return;
                }

                if (clients[clientId].udp.EndPoint.ToString() == clientEndPoint.ToString())
                {
                    clients[clientId].udp.HandleData(packet);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving UDP data: {ex}");
        }
    }

    public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
    {
        try
        {
            if (clientEndPoint != null)
            {
                udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
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
                { (int)ClientToGameRoom.welcomeReceived, ServerHandler.WelcomeReceived },
                { (int)ClientToGameRoom.playerMovement, ServerHandler.PlayerMovement },
                { (int)ClientToGameRoom.playerShooting, ServerHandler.PlayerShooting },
                { (int)ClientToGameRoom.playerThrowItem, ServerHandler.PlayerThrowItem },
                { (int)ClientToGameRoom.playerChangeWeapon, ServerHandler.PlayerChangeWeapon },
                { (int)ClientToGameRoom.playerRespawn, ServerHandler.PlayerRespawn },
            };

        Debug.Log("Initialized packets.");
    }
}
