using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSendClient
{
    #region SendBase
    private static void SendTCPData(Guid toClient, Packet packet)
    {
        packet.WriteLength();
        Room.Clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(Guid toClient, Packet packet)
    {
        packet.WriteLength();
        Room.Clients[toClient].udp.SendClientData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        foreach (var client in Room.Clients.Values)
        {
            client.tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(Guid exceptClient, Packet packet)
    {
        packet.WriteLength();
        foreach (var client in Room.Clients.Values)
        {
            if (client.Id != exceptClient)
            {
                client.tcp.SendData(packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        foreach (var client in Room.Clients.Values)
        {
            client.udp.SendClientData(packet);
        }
    }

    private static void SendUDPDataToAll(Guid exceptClient, Packet packet)
    {
        packet.WriteLength();
        foreach (var client in Room.Clients.Values)
        {
            if (client.Id != exceptClient)
            {
                client.udp.SendClientData(packet);
            }
        }
    }
    #endregion

    #region Packets

    public static void ItemOnMapPickup(int itemId)
    {
        using (Packet packet = new Packet((int)ToClient.itemOnMapPickup))
        { 
            packet.Write(itemId);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerBulletAmount(Guid toClient, WeaponBase weapon)
    {
        using (Packet packet = new Packet((int)ToClient.playerBulletAmount))
        {
            packet.Write(toClient);
            packet.Write((int)weapon.Kind);
            packet.Write(weapon.MaxBulletAmount);
            packet.Write(weapon.CurrentBulletAmount);

            SendTCPData(toClient, packet);
        }
    }

    public static void ItemSpawnedOnMap(ItemOnMapBase item)
    {
        using (Packet packet = new Packet((int)ToClient.itemSpawnedOnMap))
        {
            packet.Write(item.Id);
            packet.Write((int)item.Kind);
            packet.Write(item.Position);

            SendTCPDataToAll(packet);
        }
    }

    public static void Welcome(Guid toClient, string msg)
    {
        using (Packet packet = new Packet((int)ToClient.welcome))
        {
            packet.Write(msg);
            packet.Write(toClient);
            packet.Write(2);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnPlayer(Guid toClient, Player player)
    {
        using (Packet packet = new Packet((int)ToClient.spawnPlayer))
        {
            packet.Write(player.Id);
            packet.Write(player.Username);
            packet.Write(player.Team);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);
            packet.Write((int)player.WeaponController.GetCurrentWeapon().Kind);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(CharacterBase character)
    {
        using (Packet packet = new Packet((int)ToClient.playerPosition))
        {
            packet.Write(character.Id);
            packet.Write(character.transform.position);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerChooseWeapon(CharacterBase character)
    {
        using (Packet packet = new Packet((int)ToClient.playerChooseWeapon))
        {
            packet.Write(character.Id);
            packet.Write((int)character.WeaponController.GetCurrentWeapon().Kind);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerRotation(CharacterBase character)
    {
        using (Packet packet = new Packet((int)ToClient.playerRotation))
        {
            packet.Write(character.Id);
            packet.Write(character.transform.rotation);

            SendUDPDataToAll(character.Id, packet);
        }
    }

    public static void PlayerDisconnected(Guid playerId)
    {
        using (Packet packet = new Packet(ToClient.playerDisconnected))
        {
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHealth(HealthManager healthManager)
    {
        using (Packet packet = new Packet(ToClient.playerHealth))
        {
            packet.Write(healthManager.OwnerId);
            packet.Write(healthManager.CurrentHealth);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerRespawn(Player player)
    {
        using (Packet packet = new Packet(ToClient.playerRespawned))
        {
            packet.Write(player.Id);

            SendTCPDataToAll(packet);
        }
    }

    public static void CreateItemSpawner(Guid toClient, int spawnerId, Vector3 position, bool hasItem)
    {
        using (Packet packet = new Packet(ToClient.createItemSpawner))
        {
            packet.Write(spawnerId)
                  .Write(position)
                  .Write(hasItem);

            SendTCPData(toClient, packet);
        }
    }

    public static void ItemSpawned(int spawnerId)
    {
        using (Packet packet = new Packet(ToClient.itemSpawned))
        {
            packet.Write(spawnerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void ItemPickup(int spawnerId, Guid playerId)
    {
        using (Packet packet = new Packet(ToClient.itemPickup))
        {
            packet.Write(spawnerId);
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    #region Shooting

    public static void SpawnProjectile(Projectile projectile, Guid throwedById)
    {
        using (var packet = new Packet(ToClient.spawnProjectile))
        {
            packet.Write(projectile.Id);
            packet.Write(projectile.transform.position);
            packet.Write(throwedById);

            SendTCPDataToAll(packet);
        }
    }
    public static void ProjectilePosition(Projectile projectile)
    {
        using (var packet = new Packet(ToClient.projectilePosition))
        {
            packet.Write(projectile.Id);
            packet.Write(projectile.transform.position);

            SendUDPDataToAll(packet);
        }
    }

    public static void ProjectileExploded(Projectile projectile)
    {
        using (var packet = new Packet(ToClient.projectileExploded))
        {
            packet.Write(projectile.Id);
            packet.Write(projectile.transform.position);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerShootUDP(CharacterBase character)
    {
        using (var packet = new Packet(ToClient.playerShooting))
        {
            packet.Write(character.Id);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerShootTCP(CharacterBase character)
    {
        using (var packet = new Packet(ToClient.playerShooting))
        {
            packet.Write(character.Id);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHitUDP(CharacterBase character, WeaponKind hitBy, Vector3 position)
    {
        using (var packet = new Packet(ToClient.playerHit))
        {
            packet.Write(character.Id);
            packet.Write((int)hitBy);
            packet.Write(position);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerHitTCP(CharacterBase character, WeaponKind hitBy, Vector3 position)
    {
        using (var packet = new Packet(ToClient.playerHit))
        {
            packet.Write(character.Id);
            packet.Write((int)hitBy);
            packet.Write(position);

            SendTCPDataToAll(packet);
        }
    }

    public static void BotShoot(CharacterBase character)
    {
        using (var packet = new Packet(ToClient.botShoot))
        {
            packet.Write(character.Id);

            SendUDPDataToAll(packet);
        }
    }

    public static void BotHit(CharacterBase character, WeaponKind hitBy, Vector3 position)
    {
        using (var packet = new Packet(ToClient.botHit))
        {
            packet.Write(character.Id);
            packet.Write((int)hitBy);
            packet.Write(position);

            SendTCPDataToAll(packet);
        }
    }

    public static void BotChooseWeapon(CharacterBase character)
    {
        using (Packet packet = new Packet((int)ToClient.botChooseWeapon))
        {
            packet.Write(character.Id);
            packet.Write((int)character.WeaponController.GetCurrentWeapon().Kind);

            SendTCPDataToAll(packet);
        }
    }
    #endregion

    public static void UpdateFullRatingTable(Dictionary<Guid, RatingEntity> rating)
    {
        using (Packet packet = new Packet(ToClient.ratingTableInit))
        {
            packet.Write(rating.Count);

            foreach (var enity in rating.Values)
            {
                packet.Write(enity.PlayerId);
                packet.Write(enity.Username);
                packet.Write(enity.Killed);
                packet.Write(enity.Died);
                packet.Write(enity.Team);
            }

            SendTCPDataToAll(packet);
        }
    }

    public static void UpdateRatingTable(Guid playerKillerId, Guid playerDieId)
    {
        using (Packet packet = new Packet(ToClient.ratingTableUpdateKillAndDeath))
        {
            packet.Write(playerKillerId);
            packet.Write(playerDieId);

            SendTCPDataToAll(packet);
        }
    }

    public static void UpdateRatingTableBotsKills(int botsCount, Guid playerKillerId)
    {
        using (Packet packet = new Packet(ToClient.ratingTableUpdateKilledBots))
        {
            packet.Write(playerKillerId);
            packet.Write(botsCount);

            SendTCPDataToAll(packet);
        }
    }

    public static void UpdateRatingTableDeath(Guid playerDieId)
    {
        using (Packet packet = new Packet(ToClient.ratingTableUpdateDeath))
        {
            packet.Write(playerDieId);

            SendTCPDataToAll(packet);
        }
    }

    public static void AddPlayerRatingTable(Guid toClient, Player entity)
    {
        using (Packet packet = new Packet(ToClient.ratingTableNewPlayer))
        {
            packet.Write(entity.Id);
            packet.Write(entity.Username);
            packet.Write(entity.Team);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerGrenadeCount(Guid toClient, int grenadeCount)
    {
        using (Packet packet = new Packet(ToClient.playerGrenadeCount))
        {
            packet.Write(toClient);
            packet.Write(grenadeCount);

            SendTCPData(toClient, packet);
        }
    }

    public static void InitMap(Guid toClient, string mapString)
    {
        using (Packet packet = new Packet(ToClient.initMap))
        {
            packet.Write(mapString);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnBot(DefaultBot bot)
    {
        using (Packet packet = new Packet(ToClient.spawnBot))
        {
            packet.Write(bot.Id);
            packet.Write(bot.transform.position);
            packet.Write((int)bot.WeaponController.GetCurrentWeapon().Kind);

            SendTCPDataToAll(packet);
        }
    }

    public static void SpawnBot(Guid toClient, DefaultBot bot)
    {
        using (Packet packet = new Packet(ToClient.spawnBot))
        {
            packet.Write(bot.Id);
            packet.Write(bot.transform.position);
            packet.Write((int)bot.WeaponController.GetCurrentWeapon().Kind);

            SendTCPData(toClient, packet);
        }
    }

    public static void BotPosition(DefaultBot bot)
    {
        using (Packet packet = new Packet(ToClient.botPosition))
        {
            packet.Write(bot.Id);
            packet.Write(bot.transform.position);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerScale(Player player)
    {
        using (Packet packet = new Packet(ToClient.playerScale))
        {
            packet.Write(player.transform.lossyScale);
            packet.Write(player.Id);

            SendUDPDataToAll(packet);
        }
    }

    public static void BotRotation(DefaultBot bot)
    {
        using (Packet packet = new Packet(ToClient.botRotation))
        {
            packet.Write(bot.Id);
            packet.Write(bot.transform.rotation);

            SendUDPDataToAll(packet);
        }
    }

    public static void BotHealth(HealthManager healthManager)
    {
        using (Packet packet = new Packet(ToClient.botHealth))
        {
            packet.Write(healthManager.OwnerId);
            packet.Write(healthManager.CurrentHealth);

            SendTCPDataToAll(packet);
        }
    }

    public static void StageTime(long timeTicks)
    {
        using (Packet packet = new Packet(ToClient.stageTime))
        {
            packet.Write(timeTicks);

            SendTCPDataToAll(packet);
        }
    }

    public static void StageChanged(int stageId)
    {
        using (Packet packet = new Packet(ToClient.stageChanged))
        {
            packet.Write(stageId);

            SendTCPDataToAll(packet);
        }
    }

    public static void DestroyMapItem(int itemId)
    {
        using (Packet packet = new Packet(ToClient.destroyMapItem))
        {
            packet.Write(itemId);

            SendTCPDataToAll(packet);
        }
    }

    #endregion
}
