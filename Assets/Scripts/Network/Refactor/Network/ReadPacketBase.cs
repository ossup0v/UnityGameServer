using Refactor;

public abstract class ReadPacketBase
{
    protected SocketData _socketData;
    protected byte[] _packetBytes;

    public System.Guid GUID { get; set; }
    public int ReadPosition { get; protected set; }

    public void SetSocketData(ref SocketData socketData)
    {
        _socketData = socketData;
    }

    public void SetBytes(byte[] packetBytes)
    {
        _packetBytes = packetBytes;
    }

    public byte[] GetBytes()
    {
        return _packetBytes;
    }

    public void SetReadPosition(int readPosition)
    {
        ReadPosition = readPosition;
    }

    public void Reset()
    {
        ReadPosition = 0;
    }

    public void ReadGUIDAndDeserializePacket()
    {
        Reset();
        GUID = this.ReadGuid();
        DeserializePacket();
    }

    public abstract void DeserializePacket();
}
