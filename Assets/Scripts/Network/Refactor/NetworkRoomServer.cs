namespace Refactor
{
    public class NetworkRoomServer : INetworkServer
    {
        private UDPServer _udpServer;
        private TCPServer _tcpServer;

        private ServerRoomNetworkBytesReader _serverNetworkBytesReader;
        private NetworkServerPacketsSender _networkServerPacketsSender;
        private ClientsHolder _clientsHolder;

        public INetworkServerPacketsSender NetworkServerPacketsSender => _networkServerPacketsSender;
        public IPacketHandlersHolder PacketHandlersHolder => _serverNetworkBytesReader;
        public IClientsHolder ClientsHolder => _clientsHolder;

        public int BufferSize { get; } = 1024;

        public NetworkRoomServer()
        {
            _serverNetworkBytesReader = new ServerRoomNetworkBytesReader();
            _udpServer = new UDPServer(BufferSize, _serverNetworkBytesReader);
            _tcpServer = new TCPServer(BufferSize, _serverNetworkBytesReader);
            _networkServerPacketsSender = new NetworkServerPacketsSender(BufferSize, _udpServer, _tcpServer);
            _clientsHolder = new ClientsHolder();
        }

        public void Start(int port)
        {
            _udpServer.Bind(port);
            _tcpServer.Bind(port);            
        }

        public void Stop()
        {
            // TODO: всем из _clientsHolder закрыть соединения
            _udpServer.CloseConnection();
            _tcpServer.CloseConnection();
        }
    }
}
