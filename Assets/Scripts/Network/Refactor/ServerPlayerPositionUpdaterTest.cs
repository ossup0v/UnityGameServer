using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor
{
    public class ServerPlayerPositionUpdaterTest : NetworkServerMonoBehaviour<NetworkRoomServerProvider>
    {
        // TODO: для теста, удалить это
        private Dictionary<Guid, GameObject> _playerGameObjectByClientID = new Dictionary<Guid, GameObject>();

        public void AddPlayerGameObject(Guid clientID, GameObject playerGameObject)
        {
            _playerGameObjectByClientID.Add(clientID, playerGameObject);
        }

        private void FixedUpdate()
        {
            foreach (var player in _playerGameObjectByClientID)
            {
                var clientTCP = _networkServer.ClientsHolder.GetTcpClient(player.Key);
                var playerObject = player.Value;
                var position = playerObject.transform.position;
                var updateCharacterPositionWritePacket = new UpdateCharacterPositionWritePacket()
                {
                    CharacterClientID = player.Key, // TODO: тест
                    CharacterPosition = position
                };
                _networkServer.NetworkServerPacketsSender.SendTCP(clientTCP, updateCharacterPositionWritePacket);
            }
        }
    }
}