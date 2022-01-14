using System.Collections;
using UnityEngine;

namespace Refactor
{

    [CreateAssetMenu(fileName = "NetworkServerReceiver", menuName = "Network/NetworkServerReceiver", order = 0)]
    public class NetworkServerReceiver : ScriptableObject
    {
        [System.NonSerialized] private ServerNetworkPacketReceiver _serverNetworkPakcetReceiver;
        
        public void Init()
        {
            _serverNetworkPakcetReceiver = new ServerNetworkPacketReceiver();
        }
    }
}
