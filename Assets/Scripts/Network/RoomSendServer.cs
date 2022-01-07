using System;
using System.Collections.Generic;

public class RoomSendServer
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Room.Server.Tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Room.Server.Udp.SendData(packet);
    }

    public static void RoomIsLaunched()
    {
        using (Packet packet = new Packet((int)ToServerFromGameRoom.gameRoomLaunched))
        {
            packet.Write(Room.RoomId);
            packet.Write(Room.MetagameRoomId);
            packet.Write(Room.MaxPlayers);
            packet.Write(Room.PortForClients);

            SendTCPData(packet);
        }
    }

    public static void GameRoomEnd()
    {
        using (Packet packet = new Packet((int)ToServerFromGameRoom.gameSessionEnded))
        {
            packet.Write(Room.PortForClients);
            packet.Write(Room.MetagameRoomId);

            var teams = new Dictionary<int, TeamScore>(4);

            foreach (var entity in RatingManager.Rating.Values)
            {
                var team = entity.Team;
                if (!teams.ContainsKey(team))
                    teams.Add(team, new TeamScore());

                teams[team].Team = team;
                teams[team].DeadPlayers += entity.Died;
                teams[team].KilledMobs += entity.KilledBots;
                teams[team].KilledPlayers += entity.Killed;
                teams[team].PlayerIds.Add(entity.PlayerId);
            }

            packet.Write(teams.Count);

            foreach (var team in teams.Values)
            {
                packet.Write(team.PlayerIds.Count);

                foreach (var playerId in team.PlayerIds)
                {
                    packet.Write(playerId);
                }

                packet.Write(team.Team);
                packet.Write(team.Plase);
                packet.Write(team.KilledMobs);
                packet.Write(team.KilledPlayers);
                packet.Write(team.DeadPlayers);
            }


            SendTCPData(packet);
        }
    }
    public class TeamScore
    {
        public int Team { get; set; }
        public int Plase { get; set; }
        public List<Guid> PlayerIds { get; set; } = new List<Guid>();
        public int KilledMobs { get; set; }
        public int KilledPlayers { get; set; }
        public int DeadPlayers { get; set; }

        public override string? ToString()
        {
            return $"{nameof(Team)} {Team} " +
                $"{nameof(Plase)} {Plase} " +
                $"{nameof(PlayerIds)} {string.Join(";", PlayerIds)} " +
                $"{nameof(KilledMobs)} {KilledMobs} " +
                $"{nameof(KilledPlayers)} {KilledPlayers} " +
                $"{nameof(DeadPlayers)} {DeadPlayers}";
        }
    }
}
