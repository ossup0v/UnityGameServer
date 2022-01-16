using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Refactor
{
    public sealed class NetworkServerPacketsSender : INetworkServerPacketsSender
    {
        private readonly int _bufferSize;
        private UDPServer _udpServer;
        private TCPServer _tcpServer;

        public NetworkServerPacketsSender(int bufferSize, UDPServer udpServer, TCPServer tcpServer)
        {
            _bufferSize = bufferSize;
            _udpServer = udpServer;
            _tcpServer = tcpServer;
        }

        public void SendTCP(TcpClient tcpClient, WritePacketBase writePacket)
        {
            var bytesToSend = GetBytesToSend(writePacket);
            _tcpServer.Send(tcpClient, bytesToSend);
        }

        public void SendUDP(IPEndPoint ipEndPoint, WritePacketBase writePacket)
        {
            var bytesToSend = GetBytesToSend(writePacket);
            _udpServer.Send(ipEndPoint, bytesToSend);
        }

        private byte[] GetBytesToSend(WritePacketBase writePacket)
        {
            writePacket.SetBytes(new byte[_bufferSize]);
            writePacket.WriteBasePacketDataAndSerializePacket();
            var bytesToSend = new byte[writePacket.Lenght];
            Array.Copy(writePacket.GetBytes(), 0, bytesToSend, 0, writePacket.Lenght);
            return bytesToSend;
        }
    }
}
