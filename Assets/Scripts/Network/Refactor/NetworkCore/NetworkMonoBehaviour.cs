using UnityEngine;

public abstract class NetworkMonoBehaviour<T> : MonoBehaviour, IPacketReceivable<T> where T : ReadPacketBase
{
    protected abstract IPacketHandlersHolder _packetHandlersHolder { get; }
    protected abstract int _packetID { get; }

    public abstract void ReceivePacket(T packet);

    protected virtual void Awake()
    {
        SubscribeToPacketHandler();
    }

    protected virtual void SubscribeToPacketHandler()
    {
        var packetHandler = _packetHandlersHolder.GetPacketHandlerByPacketID(_packetID) as NetworkReadPacketHandler<T>;
        packetHandler.SubscribeToPacketHandler(this);
    }

    protected virtual void UnsubscribeFromPacketHandler()
    {
        var packetHandler = _packetHandlersHolder.GetPacketHandlerByPacketID(_packetID) as NetworkReadPacketHandler<T>;
        packetHandler.UnsubscribeFromPacketHandler(this);
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeFromPacketHandler();
    }
}
