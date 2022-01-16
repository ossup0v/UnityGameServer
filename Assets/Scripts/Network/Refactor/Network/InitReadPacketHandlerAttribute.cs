using System;

[AttributeUsage(AttributeTargets.Class)]
public class InitReadPacketHandler : Attribute
{
    public Type PacketHandlerType { get; private set; }

    public InitReadPacketHandler(Type packetHandlerType)
    {
        PacketHandlerType = packetHandlerType;
    }
}