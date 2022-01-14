using System;

[AttributeUsage(AttributeTargets.Class)]
public class NetworkPacketAttribute : Attribute
{
    public int PacketID { get; private set; }
    public Type PacketHandler { get; private set; }

    public NetworkPacketAttribute(int packetID, Type packetHandler)
    {
        PacketID = packetID;
        PacketHandler = packetHandler;
    }
}