using System;
using System.Text;
using UnityEngine;

public static class PacketWriteHelper
{
    public static void Write(this WritePacketBase packetBase, int value)
    {
        var buffer = packetBase.GetBytes();
        var writePosition = packetBase.WritePosition;
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, buffer, writePosition, bytes.Length);
        writePosition += bytes.Length;
        packetBase.SetWritePosition(writePosition);
        packetBase.SetBytes(buffer);
    }

    public static void Write(this WritePacketBase packetBase, Guid value)
    {
        foreach (var @int in Guid2Int(value))
        {
            Write(packetBase, @int);
        }
    }

    public static void Write(this WritePacketBase packetBase, string value)
    {
        Write(packetBase, value.Length);
        var buffer = packetBase.GetBytes();
        var writePosition = packetBase.WritePosition;
        var bytes = Encoding.ASCII.GetBytes(value);
        Array.Copy(bytes, 0, buffer, writePosition, bytes.Length);
    }

    public static int[] Guid2Int(Guid value)
    {
        byte[] b = value.ToByteArray();
        int bint = BitConverter.ToInt32(b, 0);
        var bint1 = BitConverter.ToInt32(b, 4);
        var bint2 = BitConverter.ToInt32(b, 8);
        var bint3 = BitConverter.ToInt32(b, 12);
        return new[] { bint, bint1, bint2, bint3 };
    }

    public static void Write(this WritePacketBase packetBase, Vector3 value)
    {
        Write(packetBase, value.x);
        Write(packetBase, value.y);
        Write(packetBase, value.z);
    }

    public static void Write(this WritePacketBase packetBase, float value)
    {
        var buffer = packetBase.GetBytes();
        var writePosition = packetBase.WritePosition;
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, buffer, writePosition, bytes.Length);
        writePosition += bytes.Length;
        packetBase.SetWritePosition(writePosition);
        packetBase.SetBytes(buffer);
    }
}
