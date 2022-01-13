using System.Net;

namespace Refactor
{
    public interface IBytesReadable
    {
        void ReadBytes(ref SocketData socketServerData, byte[] bytes);
    }
}