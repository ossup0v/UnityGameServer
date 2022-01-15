using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class PacketHandlersHolderHelper
{
    public static void FindAllPacketHandlersFor(Dictionary<int, IPacketHandleable> packetHandlersByPacketID, Type packetHandlersHolderType)
    {
        Logger.WriteLog(nameof(FindAllPacketHandlersFor), $"Searching packet handlers for {packetHandlersHolderType}");
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var assemblyType in assembly.GetTypes())
            {
                var networkPacketAttribute = assemblyType.GetCustomAttribute(typeof(NetworkPacketAttribute), false) as NetworkPacketAttribute;
                if (networkPacketAttribute != null)
                {
                    var isHasInterface = networkPacketAttribute.PacketHandler.GetInterfaces().Contains(typeof(IPacketHandlersHolder));
                    if (isHasInterface)
                    {
                        if (networkPacketAttribute.PacketHandler == packetHandlersHolderType)
                        {
                            var packetID = networkPacketAttribute.PacketID;
                            var packetHandler = Activator.CreateInstance(assemblyType) as IPacketHandleable;
                            Logger.WriteLog(nameof(FindAllPacketHandlersFor), $"Found {assemblyType} with packetID {packetID} for {packetHandlersHolderType}");
                            packetHandlersByPacketID.Add(packetID, packetHandler);
                        }
                    }
                    else
                    {
                        Logger.WriteError(nameof(FindAllPacketHandlersFor), $"{assemblyType} has not implementing {nameof(IPacketHandlersHolder)} interface");
                    }
                }
            }
        }
    }
}