using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public abstract class NetworkServerMonoBehaviour<T> : MonoBehaviour where T : NetworkServerProvider
    {
        [SerializeField] private NetworkServerProvider _networkServerProvider;
        protected INetworkServer _networkServer => _networkServerProvider.NetworkServer;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_networkServerProvider == null)
            {
                _networkServerProvider = Resources.Load<T>(typeof(T).Name);
            }
        }
#endif
    }
}