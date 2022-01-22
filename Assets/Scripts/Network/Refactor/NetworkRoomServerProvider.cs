using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    [CreateAssetMenu(fileName = "NetworkRoomServerProvider", menuName = "Network/NetworkRoomServerProvider", order = 0)]
    public class NetworkRoomServerProvider : NetworkServerProvider
    {
        private NetworkRoomServer _networkRoomServer;
        public override INetworkServer NetworkServer => _networkRoomServer;

        public void CreateNetworkRoomServer()
        {
            _networkRoomServer = new NetworkRoomServer();
        }
    }
}
