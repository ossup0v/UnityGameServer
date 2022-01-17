namespace Refactor
{
    public abstract class PacketReceiverMainThreadBase<T> : IPacketReceivable<T> where T : ReadPacketBase
    {
        protected IPacketHandlersHolder _packetHandlersHolder;
        protected abstract int _packetID { get; }

        protected PacketReceiverMainThreadBase(IPacketHandlersHolder packetHandlersHolder)
        {
            _packetHandlersHolder = packetHandlersHolder;
            SubscribeToPacketHandler();
        }

        public void ReceivePacket(T packet)
        {
            ThreadManager.ExecuteOnMainThread(() =>
            {
                ReceivePacketMainThread(packet);
            });
        }

        public virtual void Dispose()
        {
            UnsubscribeFromPacketHandler();
        }

        protected abstract void ReceivePacketMainThread(T packet);

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
    }
}