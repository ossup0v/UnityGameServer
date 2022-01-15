[InitReadPacketHandler(HelloReadPacket.PacketID_1, typeof(Refactor.ServerNetworkBytesReader))]
public sealed class HelloReadPacketHandler : NetworkReadPacketHandler<HelloReadPacket>
{
    protected override HelloReadPacket CreatePacketInstance()
    {
        return new HelloReadPacket();
    }
}

public sealed class HelloReadPacket : ReadPacketBase
{
    public const int PacketID_1 = 1;

    public override void DeserializePacket()
    {
    }
}