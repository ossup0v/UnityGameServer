using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Refactor
{
    public class UDPServer
    {
        private UdpClient _udpClient;
        private IBytesReadable _bytesReadable;
        private readonly int _bufferSize;

        public UDPServer(int bufferSize, IBytesReadable bytesReadable)
        {
            _bufferSize = bufferSize;            
            _bytesReadable = bytesReadable;
        }

        public void Bind(int port)
        {
            _udpClient = new UdpClient(port);
            _udpClient.Client.ReceiveBufferSize = _bufferSize;
            _udpClient.Client.SendBufferSize = _bufferSize;
            Logger.WriteLog(nameof(UDPServer), $"UDP Server binded");
        }
        
        public void CloseConnection()
        {
            _udpClient.Close();
            _udpClient.Dispose();
            _udpClient = null;
        }

        public void Send(IPEndPoint clientEndPoint, byte[] bytes)
        {
            _udpClient.BeginSend(bytes, bytes.Length, clientEndPoint, null, null);
        }

        private void BeginReceive()
        {
            _udpClient.BeginReceive(OnReceived, null);
        }

        private void OnReceived(IAsyncResult asyncResult)
        {
            try
            {
                // TODO: переделать, нужно переиспользование IPEndPoint иначе каждый пакет новый создается
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var receivedBytes = _udpClient.EndReceive(asyncResult, ref clientEndPoint);
                BeginReceive();
                if (receivedBytes.Length == 0)
                {
                    Logger.WriteError(nameof(OnReceived), $"Received 0 bytes from {clientEndPoint.Address}:{clientEndPoint.Port}");
                    return;
                }
                Logger.WriteLog(nameof(OnReceived), $"UDP Received {receivedBytes.Length} bytes from {clientEndPoint.Address}:{clientEndPoint.Port}");
                var socketData = new SocketData(null, clientEndPoint, false);
                _bytesReadable.ReadBytes(ref socketData, receivedBytes);

            }
            catch (Exception exception)
            {
                Logger.WriteError(nameof(OnReceived), exception.Message);
                CloseConnection();
            }
        }
    }
}