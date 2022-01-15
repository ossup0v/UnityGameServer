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
                            var packetID = initReadPacketHandler.PacketID;
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

    public static List<IPacketReceiver> Test(IPacketHandlersHolder packetHandlersHolder, Type packetHandlersHolderType)
    {
        var result = new List<IPacketReceiver>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var assemblyType in assembly.GetTypes())
            {
                var initPacketReceiverAttribute = assemblyType.GetCustomAttribute(typeof(InitPacketReceiverAttribute), false) as InitPacketReceiverAttribute;
                if (initPacketReceiverAttribute != null)
                {
                    var isHasInterface = initPacketReceiverAttribute.PacketHandlerType.GetInterfaces().Contains(typeof(IPacketHandlersHolder));
                    if (isHasInterface)
                    {
                        if (initPacketReceiverAttribute.PacketHandlerType == packetHandlersHolderType)
                        {
                            var packetReceiver = Activator.CreateInstance(assemblyType, packetHandlersHolder) as IPacketReceiver;
                            // Logger.WriteLog(nameof(FindAllPacketHandlersFor), $"Found {assemblyType} with packetID {packetID} for {packetHandlersHolderType}");
                            result.Add(packetReceiver);
                        }
                    }
                    else
                    {
                        Logger.WriteError(nameof(FindAllPacketHandlersFor), $"{assemblyType} has not implementing {nameof(IPacketHandlersHolder)} interface");
                    }
                }
            }
        }
        return result;
    }
}