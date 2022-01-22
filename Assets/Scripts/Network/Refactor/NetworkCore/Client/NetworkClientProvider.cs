using UnityEngine;

namespace Refactor
{
    public abstract class NetworkClientProvider : ScriptableObject
    {
        public abstract INetworkClient NetworkClient { get; }
    }
}