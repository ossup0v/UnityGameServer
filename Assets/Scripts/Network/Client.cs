using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
    public static int dataBufferSize = 4096;

    public Guid Id;
    public Player player;
    public TCP tcp;
    public UDP udp;

#warning всем костылям костыль
    public void SetNewId(Guid id)
    {
        Id = id;
        tcp.SetNewId(id);
        udp.SetNewId(id);
    }

    public Client(Guid clientId)
    {
        Id = clientId;
        tcp = new TCP(Id);
        udp = new UDP(Id);
    }

    public class TCP : IDisposable
    {
        public TcpClient Socket;

        private Guid _id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void SetNewId(Guid id)
        {
            _id = id;
        }

        public TCP(Guid id)
        {
            _id = id;
        }

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = dataBufferSize;
            Socket.SendBufferSize = dataBufferSize;

            stream = Socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            RoomSendClient.Welcome(_id, "Welcome to the server!");
        }

        public void SendData(Packet packet)
        {
            try
            {
                if (Socket != null && Socket.Connected)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data to player {_id} via TCP: {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    Room.Clients[_id].Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                receivedData.Reset(HandleData(data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Room.Clients[_id].Disconnect();
                Debug.Log($"Error receiving TCP data: {ex}");
            }
        }

        private bool HandleData(byte[] data)
        {
            int packetLength = 0;

            receivedData.SetBytes(data);

            if (receivedData.UnreadLength() >= 4)
            {
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
            {
                byte[] packetBytes = receivedData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        Room.packetHandlers[packetId](_id, packet);
                    }
                });

                packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            stream.Dispose();
            Socket.Dispose();
        }
    }

    public class UDP
    {
        public IPEndPoint EndPoint;

        private Guid _id;

        public UDP(Guid id)
        {
            _id = id;
        }

        public void SetNewId(Guid id)
        {
            _id = id;
        }
        
        public void Connect(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        public void SendClientData(Packet packet)
        {
            Room.SendClientUDPData(EndPoint, packet);
        }

        public void HandleData(Packet packetData)
        {
            int packetLength = packetData.ReadInt();
            byte[] packetBytes = packetData.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Room.packetHandlers[packetId](_id, packet);
                }
            });
        }
    }

    public void SendIntoGame(string playerName, int team)
    {
        Debug.Log("Send into game");

        player = NetworkManager.Instance.InstantiatePlayer(team);
        player.Initialize(Id, playerName, team);


        foreach (Client client in Room.Clients.Values)
        {
            if (client.player != null)
            {
                if (client.Id != Id)
                {
                    RoomSendClient.SpawnPlayer(Id, client.player);
                }
            }
        }

        RatingManager.InitPlayer(player);
        RoomSendClient.InitMap(Id, MapSaveManager.Instance.GetCachedObjects());

        foreach (Client client in Room.Clients.Values)
        {
            if (client.player != null)
            {
                RoomSendClient.SpawnPlayer(client.Id, player);
            }
        }

        foreach (var itemSpawner in ItemSpawner.ItemSpawners.Values)
        {
            RoomSendClient.CreateItemSpawner(Id, itemSpawner.SpawnerId, itemSpawner.transform.position, itemSpawner.HasItem);
        }

        foreach (var bot in BotManager.GetBots().Values)
        {
            RoomSendClient.SpawnBot(Id, bot);
        }
    }

    private void Disconnect()
    {
        Debug.Log($"{tcp.Socket.Client.RemoteEndPoint} was disconnected");

        ThreadManager.ExecuteOnMainThread(() =>
        {
            RatingManager.RemovePlayer(player);
            RoomSendClient.UpdateFullRatingTable(RatingManager.Rating);
            UnityEngine.Object.Destroy(player.gameObject);
            player = null;
        });

        tcp.Dispose();

        RoomSendClient.PlayerDisconnected(Id);
    }
}
