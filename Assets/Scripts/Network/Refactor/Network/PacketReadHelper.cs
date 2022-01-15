using System;
using System.Text;
using UnityEngine;

public static class PacketReadHelper
{
    public static string ReadString(this ReadPacketBase packetBase)
    {
        var stringLenght = ReadInt(packetBase);
        var buffer = packetBase.GetBytes();
        var readPosition = packetBase.ReadPosition;
        if (readPosition + stringLenght <= buffer.Length)
        {
            var stringValue = Encoding.ASCII.GetString(buffer, readPosition, stringLenght);
            readPosition += stringLenght;
            packetBase.SetReadPosition(readPosition);
            return stringValue;
        }
        else
        {
            Logger.WriteError(nameof(ReadString), $"Can't read string with lenght {stringLenght}, buffer lenght is {buffer.Length} and read position is {readPosition}");
            return default;
        }
    }

    public static int ReadInt(this ReadPacketBase packetBase)
    {
        var buffer = packetBase.GetBytes();
        var typeSize = 4;
        var readPosition = packetBase.ReadPosition;
        if (readPosition + typeSize <= buffer.Length)
        {
            var value = BitConverter.ToInt32(buffer, readPosition);
            readPosition += typeSize;
            packetBase.SetReadPosition(readPosition);
            return value;
        }
        else
        {
            Logger.WriteError(nameof(ReadInt), $"Can't read int 32, buffer lenght is {buffer.Length} and read position is {readPosition}");
            return default;
        }
    }

    public static Guid ReadGuid(this ReadPacketBase packetBase)
    {
        var value = ReadInt(packetBase);
        var value1 = ReadInt(packetBase);
        var value2 = ReadInt(packetBase);
        var value3 = ReadInt(packetBase);
        return Int2Guid(value, value1, value2, value3);
    }

    public static Guid Int2Guid(int value, int value1, int value2, int value3)
    {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        BitConverter.GetBytes(value1).CopyTo(bytes, 4);
        BitConverter.GetBytes(value2).CopyTo(bytes, 8);
        BitConverter.GetBytes(value3).CopyTo(bytes, 12);
        return new Guid(bytes);
    }

    public static Vector3 ReadVector3(this ReadPacketBase packetBase)
    {
        var x = ReadFloat(packetBase);
        var y = ReadFloat(packetBase);
        var z = ReadFloat(packetBase);
        return new Vector3(x, y, z);
    }

    public static float ReadFloat(this ReadPacketBase packetBase)
    {
        var buffer = packetBase.GetBytes();
        var typeSize = 4;
        var readPosition = packetBase.ReadPosition;
        if (readPosition + typeSize <= buffer.Length)
        {
            var value = BitConverter.ToSingle(buffer, readPosition);
            readPosition += typeSize;
            packetBase.SetReadPosition(readPosition);
            return value;
        }
        else
        {
            Logger.WriteError(nameof(ReadFloat), $"Can't read flaot, buffer lenght is {buffer.Length} and read position is {readPosition}");
            return default;
        }
    }
}