using System.Collections;
using UnityEngine;

namespace Refactor
{
    public class NetworkServerIniter : MonoBehaviour
    {
        [SerializeField] private NetworkRoomServerProvider _networkRoomServerProvider;
        [SerializeField] private int _roomServerPort = 29000;

        private void Awake()
        {
            _networkRoomServerProvider.CreateNetworkRoomServer();
            _networkRoomServerProvider.NetworkServer.Start(_roomServerPort);
        }

        private void OnDestroy()
        {
            _networkRoomServerProvider.NetworkServer.Stop();
        }
    }
}