using System;
using System.Collections.Generic;

namespace Refactor
{
    public abstract class NetworkBytesReader : IBytesReadable, IPacketHandlersHolder
    {
        private Dictionary<int, IPacketHandleable> _packetHandlersByPacketID = new Dictionary<int, IPacketHandleable>();

        public NetworkBytesReader()
        {
            PacketHandlersHolderHelper.FindAllPacketHandlersFor(_packetHandlersByPacketID, this.GetType());
        }

        public void ReadBytes(ref SocketData socketData, byte[] bytes)
        {
            var currentOffset = 0;
            var packetHeader = ReadPacketHeader(bytes, ref currentOffset);
            var packetID = packetHeader.PacketID;
            Logger.WriteLog(nameof(ReadBytes), $"Received packet with ID <b>{packetHeader.PacketID}</b>");
            if (IsPacketHandlersContainsPacketID(packetID))
            {
                GetPacketHandlerByPacketID(packetID).HandleBytes(ref socketData, bytes, currentOffset);
            }
            else
            {
                PrintCantFindPacket(packetID);
            }
        }

        public IPacketHandleable GetPacketHandlerByPacketID(int packetID)
        {
            if (_packetHandlersByPacketID.TryGetValue(packetID, out var packetHandler))
            {
                return packetHandler;
            }
            else
            {
                PrintCantFindPacket(packetID);
                return default;
            }
        }

        private PacketHeader ReadPacketHeader(byte[] bytes, ref int currentOffset)
        {
            var packetID = BitConverter.ToInt32(bytes, currentOffset);
            currentOffset += 4;
            var packetHeader = new PacketHeader(packetID);
            return packetHeader;
        }

        private void PrintCantFindPacket(int packetID)
        {
            Logger.WriteError(this.GetType().Name, $"Can't find packet handler with ID {packetID}");
        }

        private bool IsPacketHandlersContainsPacketID(int packetID)
        {
            return _packetHandlersByPacketID.ContainsKey(packetID);
        }
    }
}
