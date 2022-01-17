public interface IPacketHandlersHolder
{
    IPacketHandleable GetPacketHandlerByPacketID(int packetID);
}