using System;

[AttributeUsage(AttributeTargets.Class)]
public class InitReadPacketHandler : Attribute
{
    public Type PacketHandlersHolder { get; private set; }

    public InitReadPacketHandler(Type packetHandlersHolder)
    {
        PacketHandlersHolder = packetHandlersHolder;
    }
}