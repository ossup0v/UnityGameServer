using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public abstract class NetworkServerProvider : ScriptableObject
    {
        public abstract INetworkServer NetworkServer { get; }
    }
}
