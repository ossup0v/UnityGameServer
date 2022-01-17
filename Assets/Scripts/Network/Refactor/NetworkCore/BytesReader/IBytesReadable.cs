using System.Net;

namespace Refactor
{
    public interface IBytesReadable
    {
        void ReadBytes(ref SocketData socketData, byte[] bytes);
        void Dispose();
    }
}