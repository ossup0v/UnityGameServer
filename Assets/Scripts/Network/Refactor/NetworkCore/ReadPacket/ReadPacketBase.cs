using Refactor;

public abstract class ReadPacketBase
{
    protected byte[] _packetBytes;

    public System.Guid ClientID { get; set; }
    public int ReadPosition { get; protected set; }
    public SocketData SocketData { get; protected set; }

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

    public void SetReadPosition(int readPosition)
    {
        ReadPosition = readPosition;
    }

    public void Reset()
    {
        ReadPosition = 0;
    }

    public void ReadClientIDAndDeserializePacket()
    {
        ClientID = this.ReadGuid();
        DeserializePacket();
    }

    public abstract void DeserializePacket();
}
