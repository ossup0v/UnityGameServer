namespace Refactor
{
    [InitReadPacketHandler(typeof(Refactor.ServerRoomNetworkBytesReader))]
    public sealed class GetClientCharacterDataPacketHandler : NetworkReadPacketHandler<GetClientCharacterDataReadPacket>
    {
        public override int PacketID => GetClientCharacterDataReadPacket.PacketID_2;

        protected override GetClientCharacterDataReadPacket CreatePacketInstance()
        {
            return new GetClientCharacterDataReadPacket();
        }
    }

    public sealed class GetClientCharacterDataReadPacket : ReadPacketBase
    {
        public const int PacketID_2 = 2;

        public override void DeserializePacket()
        {
        }
    }
}