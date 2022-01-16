using System;
using System.Net;
using System.Net.Sockets;

namespace Refactor
{
    public sealed class UDPClient
    {
        private UdpClient _udpClient;
        private IPEndPoint _endPoint;
        private IBytesReadable _bytesReadable;
        private readonly int _bufferSize;

        public event Action ConnectedToServer;

        public UDPClient(int bufferSize, IBytesReadable bytesReadable)
        {
            _bufferSize = bufferSize;            
            _bytesReadable = bytesReadable;
        }

        public void Connect(string ip, int port)
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _udpClient = new UdpClient();
            _udpClient.Client.ReceiveBufferSize = _bufferSize;
            _udpClient.Client.SendBufferSize = _bufferSize;
            _udpClient.Connect(_endPoint);
            Logger.WriteLog(nameof(Connect), $"UDP connecting to server {ip}:{port}");
            BeginReceive();
            ConnectedToServer?.Invoke();
        }

        public void CloseConnection()
        {
            _udpClient.Close();      
            _udpClient.Dispose(); 
            _udpClient = null;     
        }

        public void Send(byte[] bytes)
        {
            if (_udpClient != null)
            {
                _udpClient.BeginSend(bytes, bytes.Length, null ,null);
            }
        }

        private void BeginReceive()
        {
            _udpClient.BeginReceive(OnReceived, null);
        }

        private void OnReceived(IAsyncResult asyncResult)
        {
            try
            {
                if (_udpClient == null)
                {
                    return;
                }
                var receivedBytes = _udpClient.EndReceive(asyncResult, ref _endPoint);

                if (receivedBytes.Length == 0)
                {
                    Logger.WriteError(nameof(OnReceived), "Received 0 bytes");
                    CloseConnection();
                    return;    
                }
                Logger.WriteLog(nameof(OnReceived), $"UDP Received {receivedBytes.Length} bytes");
                BeginReceive();

                var socketData = new SocketData(null, _endPoint, false);
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