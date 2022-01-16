using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Refactor;

public static class PacketHandlersHolderHelper
{
    public static void FindAllPacketHandlersFor(Dictionary<int, IPacketHandleable> packetHandlersByPacketID, Type packetHandlersHolderType)
    {
        Logger.WriteLog(nameof(FindAllPacketHandlersFor), $"Searching packet handlers for {packetHandlersHolderType}");
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var assemblyType in assembly.GetTypes())
            {
                var initReadPacketHandler = assemblyType.GetCustomAttribute(typeof(InitReadPacketHandler), false) as InitReadPacketHandler;
                if (initReadPacketHandler != null)
                {
                    var isHasInterface = initReadPacketHandler.PacketHandlerType.GetInterfaces().Contains(typeof(IPacketHandlersHolder));
                    if (isHasInterface)
                    {
                        if (initReadPacketHandler.PacketHandlerType == packetHandlersHolderType)
                        {
                            var packetHandler = Activator.CreateInstance(assemblyType) as IPacketHandleable;
                            var packetID = packetHandler.PacketID;;
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