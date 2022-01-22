using System;
using UnityEngine;

namespace Refactor
{
    public class UpdateCharacterPositionWritePacket : WritePacketBase
    {
        public Guid CharacterClientID { get; set; }
        public Vector3 CharacterPosition { get; set; }

        public override int PacketID => 3;

        public override void SerializePacket()
        {
            this.Write(CharacterClientID);
            this.Write(CharacterPosition);    
        }
    }
}