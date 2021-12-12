using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    #region SendBase
    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }
    #endregion

    #region Packets
    public static void Welcome(int toClient, string msg)
    {
        using (Packet packet = new Packet((int)ServerPackets.welcome))
        {
            packet.Write(msg);
            packet.Write(toClient);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            packet.Write(player.Id);
            packet.Write(player.Username);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write((int)player.WeaponController.GetCurrentWeapon().Kind);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerPosition))
        {
            packet.Write(player.Id);
            packet.Write(player.transform.position);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerChooseWeapon(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerChooseWeapon))
        {
            packet.Write(player.Id);
            packet.Write((int)player.WeaponController.GetCurrentWeapon().Kind);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerRotation(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerRotation))
        {
            packet.Write(player.Id);
            packet.Write(player.transform.rotation);

            SendUDPDataToAll(player.Id, packet);
        }
    }

    public static void PlayerDisconnected(int playerId)
    {
        using (Packet packet = new Packet(ServerPackets.playerDisconnected))
        {
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHealth(Player player)
    {
        using (Packet packet = new Packet(ServerPackets.playerHealth))
        {
            packet.Write(player.Id);
            packet.Write(player.CurrentHealth);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerRespawn(Player player)
    {
        using (Packet packet = new Packet(ServerPackets.playerRespawned))
        {
            packet.Write(player.Id);

            SendTCPDataToAll(packet);
        }
    }

    public static void CreateItemSpawner(int toClient, int spawnerId, Vector3 position, bool hasItem)
    {
        using (Packet packet = new Packet(ServerPackets.createItemSpawner))
        {
            packet.Write(spawnerId)
                  .Write(position)
                  .Write(hasItem);

            SendTCPData(toClient, packet);
        }
    }

    public static void ItemSpawned(int spawnerId)
    {
        using (Packet packet = new Packet(ServerPackets.itemSpawned))
        {
            packet.Write(spawnerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void ItemPickup(int spawnerId, int playerId)
    {
        using (Packet packet = new Packet(ServerPackets.itemPickup))
        {
            packet.Write(spawnerId);
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    #region Shooting

    public static void SpawnProjectile(Projectile projectile, int throwedById)
    {
        using (var packet = new Packet(ServerPackets.spawnProjectile))
        {
            packet.Write(projectile.Id);
            packet.Write(projectile.transform.position);
            packet.Write(throwedById);

            SendTCPDataToAll(packet);
        }
    }
    public static void ProjectilePosition(Projectile projectile)
    {
        using (var packet = new Packet(ServerPackets.projectilePosition))
        {
            packet.Write(projectile.Id);
            packet.Write(projectile.transform.position);

            SendUDPDataToAll(packet);
        }
    }

    public static void ProjectileExploded(Projectile projectile)
    {
        using (var packet = new Packet(ServerPackets.projectileExploded))
        {
            packet.Write(projectile.Id);
            packet.Write(projectile.transform.position);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerShootUDP(Player player)
    {
        using (var packet = new Packet(ServerPackets.playerShooting))
        {
            packet.Write(player.Id);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerShootTCP(Player player)
    {
        using (var packet = new Packet(ServerPackets.playerShooting))
        {
            packet.Write(player.Id);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHitUDP(Player player, WeaponKind hitBy, Vector3 position)
    {
        using (var packet = new Packet(ServerPackets.playerHit))
        {
            packet.Write(player.Id);
            packet.Write((int) hitBy);
            packet.Write(position);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerHitTCP(Player player, WeaponKind hitBy, Vector3 position)
    {
        using (var packet = new Packet(ServerPackets.playerHit))
        {
            packet.Write(player.Id);
            packet.Write((int) hitBy);
            packet.Write(position);

            SendTCPDataToAll(packet);
        }
    }
    #endregion

    public static void InitRatingTable(int toClient, Dictionary<int, RatingEntity> rating)
    {
        using (Packet packet = new Packet(ServerPackets.ratingTableInit))
        {
            packet.Write(rating.Count);
            
            foreach (var enity in rating.Values)
            {
                packet.Write(enity.PlayerId);
                packet.Write(enity.Username);
                packet.Write(enity.Killed);
                packet.Write(enity.Died);
            }

            SendTCPData(toClient, packet);
        }
    }

    public static void UpdateRatingTable(int playerKillerId, int playerDieId)
    {
        using (Packet packet = new Packet(ServerPackets.ratingTableUpdate))
        {
            packet.Write(playerKillerId);
            packet.Write(playerDieId);

            SendTCPDataToAll(packet);
        }
    }

    public static void AddPlayerRatingTable(int toClient, Player entity)
    {
        using (Packet packet = new Packet(ServerPackets.ratingTableNewPlayer))
        {
            packet.Write(entity.Id);
            packet.Write(entity.Username);

            SendTCPData(toClient, packet);
        }
    }
    #endregion
}
