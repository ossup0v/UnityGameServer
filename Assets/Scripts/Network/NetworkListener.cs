using System;
using System.Net;
using System.Net.Sockets;

public class NetworkListener
{
    private TcpListener _tcpListener;
    private UdpClient _udpListener;
    private readonly Action<IAsyncResult> _tcpCallback;
    private readonly Action<IAsyncResult> _udpCallback;
    public readonly int Port;

    public NetworkListener(int port, Action<IAsyncResult> tcpCallback, Action<IAsyncResult> udpCallback)
    {
        Port = port;
        _tcpCallback = tcpCallback;
        _udpCallback = udpCallback;
    }

    public void StartListen()
    {
        _tcpListener = new TcpListener(IPAddress.Any, Port);
        _tcpListener.Start();
        _tcpListener.BeginAcceptTcpClient(result => _tcpCallback(result), null);

        _udpListener = new UdpClient(Port);
        _udpListener.BeginReceive(result => _udpCallback(result), null);
    }

    public TcpListener GetTcpListener() => _tcpListener;
    public UdpClient GetUdpListener() => _udpListener;

    public void StopListen()
    {
        _udpListener.Close();
        _tcpListener.Stop();
    }
}
