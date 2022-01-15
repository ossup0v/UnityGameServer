using System;

[AttributeUsage(AttributeTargets.Class)]
public class InitReadPacketHandler : Attribute
{
    public int PacketID { get; private set; }
    public Type PacketHandlerType { get; private set; }

    public InitReadPacketHandler(int packetID, Type packetHandlerType)
    {
        PacketID = packetID;
        PacketHandlerType = packetHandlerType;
    }
}