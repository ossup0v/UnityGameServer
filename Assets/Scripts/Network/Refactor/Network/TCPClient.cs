using System;
using System.Net.Sockets;

namespace Refactor
{
    public sealed class TCPClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private byte[] _receiveBuffer;
        private IBytesReadable _bytesReadable;
        private readonly int _bufferSize;

        public TCPClient(int bufferSize, IBytesReadable bytesReadable)
        {
            _bufferSize = bufferSize;
            _bytesReadable = bytesReadable;
        }

        public void Connect(string ip, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.ReceiveBufferSize = _bufferSize;
            _tcpClient.SendBufferSize = _bufferSize;
            _receiveBuffer = new byte[_bufferSize];
            Logger.WriteLog(nameof(Connect), $"TCP Trying connect to server {ip}:{port}");
            _tcpClient.BeginConnect(ip, port, OnConnected, null);
        }

        public void CloseConnection()
        {
            _networkStream.Close();
            _networkStream.Dispose();
            _networkStream = null;
            _tcpClient.Close();
            _tcpClient.Dispose();
            _tcpClient = null;
        }

        public void Send(byte[] bytes)
        {
            try
            {
                if (_tcpClient != null)
                {
                    _networkStream.BeginWrite(bytes, 0, bytes.Length, null, null);
                }
            }
            catch (Exception exception)
            {
                Logger.WriteError(nameof(Send), exception.Message);
            }
        }

        private void OnConnected(IAsyncResult asyncResult)
        {
            _tcpClient.EndConnect(asyncResult);
            
            if (_tcpClient.Connected == false)
            {
                Logger.WriteError(nameof(OnConnected), "Connection not established");
                CloseConnection();
                return;
            }

            var remoteEndPoint = ((System.Net.IPEndPoint)_tcpClient.Client.RemoteEndPoint);
            Logger.WriteLog(nameof(OnConnected), $"TCP Connected to {remoteEndPoint.Address} {remoteEndPoint.Port}");

            _networkStream = _tcpClient.GetStream();
            BeginRead();
        }

        private void BeginRead()
        {
            try
            {
                _networkStream.BeginRead(_receiveBuffer, 0, _bufferSize, OnReaded, null);
            }
            catch (Exception exception)
            {
                Logger.WriteError(nameof(BeginRead), exception.Message);
            }
        }

        private void OnReaded(IAsyncResult asyncResult)
        {
            try
            {
                if (_networkStream == null)
                {
                    return;
                }
                var readedNumberOfBytes = _networkStream.EndRead(asyncResult);                
                // UnityEngine.Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId + " thread ID " + _receiveBuffer.GetHashCode());
                if (readedNumberOfBytes <= 0)
                {
                    Logger.WriteError(nameof(OnReaded), $"Readed {readedNumberOfBytes} bytes.");
                    CloseConnection();
                    return;
                }
                // TODO: нужно будет убрать создание нового массива
                var readedBytes = new byte[readedNumberOfBytes];
                Array.Copy(_receiveBuffer, readedBytes, readedNumberOfBytes);
                
                Logger.WriteLog(nameof(OnReaded), $"TCP Received {readedNumberOfBytes} bytes");
                var socketData = new SocketData(_tcpClient, null, true);
                _bytesReadable.ReadBytes(ref socketData, readedBytes);

                BeginRead();
            }
            catch (Exception exception)
            {
                Logger.WriteError(nameof(OnReaded), exception.Message);
                CloseConnection();
            }
        }
    }
}