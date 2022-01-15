using System;

[AttributeUsage(AttributeTargets.Class)]
public class InitPacketReceiverAttribute : Attribute
{
    public Type PacketHandlerType { get; private set; }

    public InitPacketReceiverAttribute(Type packetHandlerType)
    {
        PacketHandlerType = packetHandlerType;
    }
}