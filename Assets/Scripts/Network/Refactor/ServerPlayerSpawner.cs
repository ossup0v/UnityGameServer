using System;
using UnityEngine;

namespace Refactor
{
    public class ServerPlayerSpawner : NetworkServerMonoBehaviour<NetworkRoomServerProvider>
    {
        // TODO: переделать
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private ServerPlayerPositionUpdaterTest _serverPlayerPositionUpdaterTest;
        private HelloReadPacketReceiver _helloReadPacketReceiver;
        private GetClientCharacterDataPacketReceiver _getClientCharacterDataPacketReceiver;

        private void Start()
        {
            // TODO: переделать, убрать отсюда
            _helloReadPacketReceiver = new HelloReadPacketReceiver(_networkServer.ClientsHolder, _networkServer.NetworkServerPacketsSender, _networkServer.PacketHandlersHolder);
            _getClientCharacterDataPacketReceiver = new GetClientCharacterDataPacketReceiver(_networkServer.ClientsHolder, _networkServer.NetworkServerPacketsSender, _networkServer.PacketHandlersHolder);
            _getClientCharacterDataPacketReceiver.ReceivedGetClientCharacterPacket += OnReceivedGetClientCharacterPacket;
        }

        private void OnReceivedGetClientCharacterPacket(Guid clientID)
        {
            var playerGameObject = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            playerGameObject.name = clientID.ToString();
            _serverPlayerPositionUpdaterTest.AddPlayerGameObject(clientID, playerGameObject);
        }
    }
}