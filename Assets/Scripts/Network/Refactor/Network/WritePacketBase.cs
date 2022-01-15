using Refactor;

public abstract class WritePacketBase
{
    protected byte[] _packetBytes;
    
    public SocketData SocketData { get; protected set; }
    public int WritePosition { get; protected set; }
    public int Lenght => WritePosition;
    public abstract int PacketID { get; }

    public System.Guid GUID { get; set; }

    public void SetSocketData(ref SocketData socketData)
    {
        SocketData = socketData;
    }

    public void SetBytes(byte[] packetBytes)
    {
        _packetBytes = packetBytes;
    }

    public byte[] GetBytes()
    {
        return _packetBytes;
    }

    public void SetWritePosition(int readPosition)
    {
        WritePosition = readPosition;
    }

    public void Reset()
    {
        WritePosition = 0;
    }

    public void WriteBasePacketDataAndSerializePacket()
    {
        Reset();
        this.Write(PacketID);
        this.Write(GUID);
        SerializePacket();
    }

    public abstract void SerializePacket();
}
