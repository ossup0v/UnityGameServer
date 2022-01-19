namespace Refactor
{
    public abstract class PacketReceiverMainThreadBase<T> : PacketReceiverBase<T> where T : ReadPacketBase
    {
        protected PacketReceiverMainThreadBase(IPacketHandlersHolder packetHandlersHolder) : base(packetHandlersHolder)
        {
        }

        public override void ReceivePacket(T packet)
        {
            ThreadManager.ExecuteOnMainThread(() =>
            {
                ReceivePacketMainThread(packet);
            });
        }

        protected abstract void ReceivePacketMainThread(T packet);
    }
}