using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Refactor
{
    public class TCPServer
    {
        private class ObjectState
        {
            public byte[] Buffer { get; set; }
            public NetworkStream NetworkStream { get; set; }
            public TcpClient TcpClient { get; set; }
        }

        private TcpListener _tcpListener;
        private IBytesReadable _bytesReadable;
        private readonly int _bufferSize;

        private List<TcpClient> connectedTcpClients = new List<TcpClient>();

        public TCPServer(int bufferSize, IBytesReadable bytesReadable)
        {
            _bufferSize = bufferSize;
            _bytesReadable = bytesReadable;
        }

        public void Bind(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Server.ReceiveBufferSize = _bufferSize;
            _tcpListener.Server.SendBufferSize = _bufferSize;
            _tcpListener.Start();
            Logger.WriteLog(nameof(TCPServer), $"TCP Server binded");
            BeginAccept();
        }

        public void CloseConnection()
        {
            foreach (var connectedTcpClient in connectedTcpClients)
            {
                connectedTcpClient.GetStream().Close();
                connectedTcpClient.GetStream().Dispose();
                connectedTcpClient.Close();
                connectedTcpClient.Dispose();
            }
            _tcpListener.Stop();
            _tcpListener = null;
            connectedTcpClients.Clear();
        }

        public void Send(TcpClient tcpClient, byte[] bytes)
        {
            tcpClient.GetStream().BeginWrite(bytes, 0, bytes.Length, null, null);
        }

        private void BeginAccept()
        {
            _tcpListener.BeginAcceptTcpClient(OnAcceptCompleted, null);
        }

        private void OnAcceptCompleted(IAsyncResult asyncResult)
        {
            var tcpClient = _tcpListener.EndAcceptTcpClient(asyncResult);
            BeginAccept();

            var remoteEndPoint = ((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint);
            Logger.WriteLog(nameof(TCPServer), $"TCP Server accept completed {remoteEndPoint.Address}:{remoteEndPoint.Port}");
            
            var objectState = new ObjectState()
            {
                Buffer = new byte[_bufferSize],
                TcpClient = tcpClient,
                NetworkStream = tcpClient.GetStream()
            };
            BeginRead(objectState);
        }

        private void BeginRead(ObjectState objectState)
        {
            objectState.NetworkStream.BeginRead(objectState.Buffer, 0, _bufferSize, OnReaded, objectState);
        }

        private void OnReaded(IAsyncResult asyncResult)
        {
            var objectState = asyncResult.AsyncState as ObjectState;
            var readedNumberOfBytes = objectState.NetworkStream.EndRead(asyncResult);

            if (readedNumberOfBytes <= 0)
            {
                Logger.WriteError(nameof(OnReaded), $"Readed {readedNumberOfBytes} bytes. Client probably disconnected.");
                // TODO закрыть подключение для юзера
                return;
            }
            // TODO: нужно будет убрать создание нового массива
            var readedBytes = new byte[readedNumberOfBytes];
            Array.Copy(objectState.Buffer, readedBytes, readedNumberOfBytes);
            Logger.WriteLog(nameof(OnReaded), $"UDP Received {readedNumberOfBytes} bytes");
            var socketData = new SocketData(objectState.TcpClient, null, true);
            _bytesReadable.ReadBytes(ref socketData, readedBytes);
            BeginRead(objectState);
        }
    }
}