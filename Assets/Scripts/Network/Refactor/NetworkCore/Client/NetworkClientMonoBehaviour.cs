using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public abstract class NetworkClientMonoBehaviour<T> : MonoBehaviour where T : NetworkClientProvider
    {
        [SerializeField] private NetworkClientProvider _networkClientProvider;
        protected INetworkClient _networkClient => _networkClientProvider.NetworkClient;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_networkClientProvider == null)
            {
                _networkClientProvider = Resources.Load<T>(typeof(T).Name);
            }
        }
#endif
    }
}